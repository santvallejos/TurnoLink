using TurnoLink.Business.DTOs;
using TurnoLink.Business.Interfaces;
using TurnoLink.DataAccess.Data;
using TurnoLink.DataAccess.Entities;
using TurnoLink.DataAccess.Enums;
using TurnoLink.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TurnoLink.Business.Services
{
    /// <summary>
    /// Service for managing availability slots.
    /// All times are stored and processed in UTC format.
    /// Frontend is responsible for timezone conversions using the UTC offset.
    /// </summary>
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IUserRepository _userRepository;
        private readonly TurnoLinkDbContext _context;

        private const int MaxRecurringMonths = 6;

        private const int MaxRecurringSlots = 1000;

        private const int BatchSize = 100;

        public AvailabilityService(
            IAvailabilityRepository availabilityRepository,
            IServiceRepository serviceRepository,
            IUserRepository userRepository,
            TurnoLinkDbContext context)
        {
            _context = context;
            _availabilityRepository = availabilityRepository;
            _serviceRepository = serviceRepository;
            _userRepository = userRepository;
        }

        public async Task<AvailabilityDto?> GetByIdAsync(Guid id)
        {
            try
            {
                var availability = await _availabilityRepository.GetByIdAsync(id);
                if (availability == null)
                    return null;

                var service = await _serviceRepository.GetByIdAsync(availability.ServiceId);
                var isBooked = await IsAvailabilityBookedAsync(id);
                return MapToDto(availability, service, isBooked);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error retrieving availability: {e.Message}");
            }
        }

        public async Task<IEnumerable<AvailabilityDto>> GetAvailabilitiesByUserIdAsync(Guid userId)
        {
            try
            {
                var availabilities = await _availabilityRepository.GetAvailabilitiesByUserIdAsync(userId);
                var now = DateTime.UtcNow;
                var result = new List<AvailabilityDto>();

                foreach (var availability in availabilities.Where(a => a.StartTimeUtc >= now))
                {
                    var service = await _serviceRepository.GetByIdAsync(availability.ServiceId);
                    var isBooked = await IsAvailabilityBookedAsync(availability.Id);
                    result.Add(MapToDto(availability, service, isBooked));
                }

                return result.OrderBy(a => a.StartTimeUtc);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error retrieving availabilities for user: {e.Message}");
            }
        }

        public async Task<IEnumerable<AvailabilityDto>> GetAvailabilitiesByServiceIdAsync(Guid serviceId)
        {
            try
            {
                var availabilities = await _availabilityRepository.GetAvailabilitiesByServiceIdAsync(serviceId);
                var service = await _serviceRepository.GetByIdAsync(serviceId);
                var now = DateTime.UtcNow;

                var result = new List<AvailabilityDto>();
                foreach (var availability in availabilities.Where(a => a.StartTimeUtc >= now))
                {
                    var isBooked = await IsAvailabilityBookedAsync(availability.Id);
                    result.Add(MapToDto(availability, service, isBooked));
                }

                return result.OrderBy(a => a.StartTimeUtc);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error retrieving availabilities for service: {e.Message}");
            }
        }

        public async Task<IEnumerable<AvailabilityDto>> GetAvailableSlotsByDateRangeAsync(
            Guid userId,
            DateTime startDateUtc,
            DateTime endDateUtc)
        {
            try
            {
                var availabilities = await _availabilityRepository.GetAvailabilitiesByUserIdAsync(userId);
                var now = DateTime.UtcNow;

                var effectiveStartDate = startDateUtc < now ? now : startDateUtc;

                var filtered = availabilities
                    .Where(a => a.StartTimeUtc >= effectiveStartDate && a.StartTimeUtc <= endDateUtc)
                    .OrderBy(a => a.StartTimeUtc);

                var result = new List<AvailabilityDto>();
                foreach (var availability in filtered)
                {
                    var service = await _serviceRepository.GetByIdAsync(availability.ServiceId);
                    var isBooked = await IsAvailabilityBookedAsync(availability.Id);
                    result.Add(MapToDto(availability, service, isBooked));
                }

                return result;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error retrieving available slots: {e.Message}");
            }
        }

        public async Task<AvailabilityDto> CreateAvailabilityAsync(Guid userId, CreateAvailabilityDto createDto)
        {
            try
            {
                var (_, service) = await ValidateUserAndServiceAsync(userId, createDto.ServiceId);

                // Convertir fecha y hora local a UTC usando el offset proporcionado
                var startTimeUtc = ConvertLocalToUtc(createDto.StartDate, createDto.StartTime, createDto.UtcOffsetMinutes);

                ValidateStartTime(startTimeUtc);

                if (!await IsSlotAvailableAsync(userId, startTimeUtc, service.DurationMinutes))
                    throw new InvalidOperationException("This time slot overlaps with an existing availability");

                var endTimeUtc = startTimeUtc.AddMinutes(service.DurationMinutes);

                var availability = new Availability
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    ServiceId = createDto.ServiceId,
                    StartTimeUtc = startTimeUtc,
                    EndTimeUtc = endTimeUtc,
                    Repeat = RepeatAvailability.None,
                    CreatedAt = DateTime.UtcNow
                };

                await _availabilityRepository.AddAsync(availability);
                await _context.SaveChangesAsync();

                return MapToDto(availability, service, false);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error creating availability: {e.Message}");
            }
        }

        public async Task<IEnumerable<AvailabilityDto>> CreateRecurringAvailabilityAsync(Guid userId, CreateRecurringAvailabilityDto createDto)
        {
            var (_, service) = await ValidateUserAndServiceAsync(userId, createDto.ServiceId);

            if (createDto.Repeat == RepeatAvailability.None)
                throw new ArgumentException("Repeat frequency must be specified for recurring availability");

            // Convertir fecha y hora local a UTC
            var startTimeUtc = ConvertLocalToUtc(createDto.StartDate, createDto.StartTime, createDto.UtcOffsetMinutes);
            var endDateUtc = ConvertLocalToUtc(createDto.EndDate, createDto.StartTime, createDto.UtcOffsetMinutes);

            ValidateStartTime(startTimeUtc);
            ValidateRecurringDates(startTimeUtc, endDateUtc);

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var createdAvailabilities = new List<AvailabilityDto>();
                var currentUtc = startTimeUtc;
                int count = 0;

                while (currentUtc <= endDateUtc)
                {
                    if (await IsSlotAvailableAsync(userId, currentUtc, service.DurationMinutes))
                    {
                        var endTimeUtc = currentUtc.AddMinutes(service.DurationMinutes);

                        var newAvailability = new Availability
                        {
                            Id = Guid.NewGuid(),
                            UserId = userId,
                            ServiceId = createDto.ServiceId,
                            StartTimeUtc = currentUtc,
                            EndTimeUtc = endTimeUtc,
                            Repeat = createDto.Repeat,
                            CreatedAt = DateTime.UtcNow
                        };

                        await _availabilityRepository.AddAsync(newAvailability);
                        createdAvailabilities.Add(MapToDto(newAvailability, service, false));
                        count++;

                        if (count % BatchSize == 0)
                            await _context.SaveChangesAsync();
                    }

                    currentUtc = GetNextRecurringDate(currentUtc, createDto.Repeat);

                    if (count > MaxRecurringSlots)
                        throw new InvalidOperationException($"Too many availabilities would be created. Maximum is {MaxRecurringSlots}. Please adjust your date range.");
                }

                if (count % BatchSize != 0)
                    await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return createdAvailabilities;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<AvailabilityDto> UpdateAvailabilityAsync(Guid id, Guid userId, UpdateAvailabilityDto updateDto)
        {
            try
            {
                var availability = await _availabilityRepository.GetByIdAsync(id);

                if (availability == null)
                    throw new ArgumentException("Availability not found");

                if (availability.UserId != userId)
                    throw new UnauthorizedAccessException("You don't have permission to update this availability");

                var service = await _serviceRepository.GetByIdAsync(availability.ServiceId);
                if (service == null)
                    throw new ArgumentException("Service not found");

                // Si no hay cambios, devolver el estado actual
                if (!updateDto.NewDate.HasValue && !updateDto.NewTime.HasValue)
                {
                    var isBooked = await IsAvailabilityBookedAsync(availability.Id);
                    return MapToDto(availability, service, isBooked);
                }

                // Validar que se proporcione el offset si hay cambios de fecha/hora
                if (!updateDto.UtcOffsetMinutes.HasValue)
                    throw new ArgumentException("UTC offset is required when updating date or time");

                // Calcular nueva fecha/hora en UTC
                var newDate = updateDto.NewDate ?? DateOnly.FromDateTime(availability.StartTimeUtc);
                var newTime = updateDto.NewTime ?? TimeOnly.FromDateTime(availability.StartTimeUtc);
                var newStartTimeUtc = ConvertLocalToUtc(newDate, newTime, updateDto.UtcOffsetMinutes.Value);

                ValidateStartTime(newStartTimeUtc);

                if (!await IsSlotAvailableAsync(userId, newStartTimeUtc, service.DurationMinutes, id))
                    throw new InvalidOperationException("This time slot overlaps with an existing availability");

                availability.StartTimeUtc = newStartTimeUtc;
                availability.EndTimeUtc = newStartTimeUtc.AddMinutes(service.DurationMinutes);

                _availabilityRepository.Update(availability);
                await _context.SaveChangesAsync();

                var isBookedAfterUpdate = await IsAvailabilityBookedAsync(availability.Id);
                return MapToDto(availability, service, isBookedAfterUpdate);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error updating availability: {e.Message}");
            }
        }

        public async Task DeleteAvailabilityAsync(Guid id, Guid userId)
        {
            try
            {
                var availability = await _availabilityRepository.GetByIdAsync(id);

                if (availability == null)
                    throw new ArgumentException("Availability not found");

                if (availability.UserId != userId)
                    throw new UnauthorizedAccessException("You don't have permission to delete this availability");

                var hasBookings = await _context.Set<Booking>()
                    .AnyAsync(b => b.AvailabilityId == id && b.Status != BookingStatus.Canceled);

                if (hasBookings)
                    throw new InvalidOperationException("Cannot delete availability with active bookings");

                _availabilityRepository.Remove(availability);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error deleting availability: {e.Message}");
            }
        }

        /// <summary>
        /// Converts local date and time to UTC using the provided offset
        /// </summary>
        /// <param name="date">Local date</param>
        /// <param name="time">Local time</param>
        /// <param name="utcOffsetMinutes">UTC offset in minutes (negative for west of UTC, positive for east)</param>
        /// <returns>DateTime in UTC with Kind=Utc</returns>
        private static DateTime ConvertLocalToUtc(DateOnly date, TimeOnly time, int utcOffsetMinutes)
        {
            var localDateTime = date.ToDateTime(time);
            var utcDateTime = localDateTime.AddMinutes(-utcOffsetMinutes);
            // PostgreSQL requiere DateTimeKind.Utc para columnas timestamp with time zone
            return DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
        }

        /// <summary>
        /// Validates that the start time is not in the past
        /// </summary>
        private static void ValidateStartTime(DateTime startTimeUtc)
        {
            if (startTimeUtc < DateTime.UtcNow)
                throw new ArgumentException("Start time cannot be in the past");
        }

        /// <summary>
        /// Validates the date range for recurring availability
        /// </summary>
        private static void ValidateRecurringDates(DateTime startTimeUtc, DateTime endDateUtc)
        {
            if (endDateUtc <= startTimeUtc)
                throw new ArgumentException("End date must be after start date");

            var maxEndDate = startTimeUtc.AddMonths(MaxRecurringMonths);
            if (endDateUtc > maxEndDate)
                throw new ArgumentException($"Cannot create recurring availabilities beyond {MaxRecurringMonths} months");
        }

        /// <summary>
        /// Gets the next recurring date based on the repeat frequency
        /// </summary>
        private static DateTime GetNextRecurringDate(DateTime current, RepeatAvailability repeat)
        {
            return repeat switch
            {
                RepeatAvailability.Daily => current.AddDays(1),
                RepeatAvailability.Weekly => current.AddDays(7),
                RepeatAvailability.Monthly => current.AddMonths(1),
                _ => throw new ArgumentException("Invalid repeat option"),
            };
        }

        /// <summary>
        /// Checks if an availability slot has an active booking
        /// </summary>
        private async Task<bool> IsAvailabilityBookedAsync(Guid availabilityId)
        {
            return await _context.Set<Booking>()
                .AnyAsync(b => b.AvailabilityId == availabilityId
                    && b.Status != BookingStatus.Canceled
                    && b.Status != BookingStatus.NoShow);
        }

        /// <summary>
        /// Checks if a time slot is available (no overlapping availabilities)
        /// </summary>
        private async Task<bool> IsSlotAvailableAsync(Guid userId, DateTime startTimeUtc, int durationMinutes, Guid? excludeAvailabilityId = null)
        {
            var endTimeUtc = startTimeUtc.AddMinutes(durationMinutes);

            var hasOverlap = await _context.Set<Availability>()
                .AnyAsync(a =>
                    a.UserId == userId &&
                    a.Id != excludeAvailabilityId &&
                    a.StartTimeUtc < endTimeUtc &&
                    a.EndTimeUtc > startTimeUtc);

            return !hasOverlap;
        }

        /// <summary>
        /// Validates that the user and service exist and belong together
        /// </summary>
        private async Task<(User user, Service service)> ValidateUserAndServiceAsync(Guid userId, Guid serviceId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            var service = await _serviceRepository.GetByIdAsync(serviceId);
            if (service == null || service.UserId != userId)
                throw new ArgumentException("Service not found or does not belong to user");

            return (user, service);
        }

        /// <summary>
        /// Maps an Availability entity to AvailabilityDto
        /// </summary>
        private static AvailabilityDto MapToDto(Availability availability, Service? service, bool isBooked)
        {
            return new AvailabilityDto
            {
                Id = availability.Id,
                ServiceId = availability.ServiceId,
                StartTimeUtc = DateTime.SpecifyKind(availability.StartTimeUtc, DateTimeKind.Utc),
                EndTimeUtc = DateTime.SpecifyKind(availability.EndTimeUtc, DateTimeKind.Utc),
                ServiceName = service?.Name ?? string.Empty,
                DurationMinutes = service?.DurationMinutes ?? 0,
                Repeat = availability.Repeat,
                IsBooked = isBooked
            };
        }
    }
}
