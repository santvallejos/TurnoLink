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
    /// Service for managing availability slots
    /// </summary>
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IUserRepository _userRepository;
        private readonly TurnoLinkDbContext _context;

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
            var availability = await _availabilityRepository.GetByIdAsync(id);
            if (availability == null)
                return null;

            var service = await _serviceRepository.GetByIdAsync(availability.ServiceId);
            var isBooked = await IsAvailabilityBookedAsync(id);
            return MapToDto(availability, service?.Name ?? "", isBooked);
        }

        public async Task<IEnumerable<AvailabilityDto>> GetAvailabilitiesByUserIdAsync(Guid userId)
        {
            var availabilities = await _availabilityRepository.GetAvailabilitiesByUserIdAsync(userId);
            var now = DateTime.UtcNow;
            var result = new List<AvailabilityDto>();

            // Filter availabilities that haven't started yet (>= allows same minute bookings)
            foreach (var availability in availabilities.Where(a => a.StartTime >= now))
            {
                var service = await _serviceRepository.GetByIdAsync(availability.ServiceId);
                var isBooked = await IsAvailabilityBookedAsync(availability.Id);
                result.Add(MapToDto(availability, service?.Name ?? "", isBooked));
            }

            return result;
        }

        public async Task<IEnumerable<AvailabilityDto>> GetAvailabilitiesByServiceIdAsync(Guid serviceId)
        {
            var availabilities = await _availabilityRepository.GetAvailabilitiesByServiceIdAsync(serviceId);
            var service = await _serviceRepository.GetByIdAsync(serviceId);
            var now = DateTime.UtcNow;
            
            var result = new List<AvailabilityDto>();
            // Filter availabilities that haven't started yet (>= allows same minute bookings)
            foreach (var availability in availabilities.Where(a => a.StartTime >= now))
            {
                var isBooked = await IsAvailabilityBookedAsync(availability.Id);
                result.Add(MapToDto(availability, service?.Name ?? "", isBooked));
            }
            
            return result;
        }

        public async Task<IEnumerable<AvailabilityDto>> GetAvailableSlotsByDateRangeAsync(
            Guid userId, 
            DateTime startDate, 
            DateTime endDate)
        {
            var availabilities = await _availabilityRepository.GetAvailabilitiesByUserIdAsync(userId);
            var now = DateTime.UtcNow;
            
            // Ensure we don't return past dates, even if startDate is in the past
            var effectiveStartDate = startDate < now ? now : startDate;
            
            var filtered = availabilities
                .Where(a => a.StartTime >= effectiveStartDate && a.StartTime <= endDate)
                .OrderBy(a => a.StartTime);

            var result = new List<AvailabilityDto>();
            foreach (var availability in filtered)
            {
                var service = await _serviceRepository.GetByIdAsync(availability.ServiceId);
                var isBooked = await IsAvailabilityBookedAsync(availability.Id);
                result.Add(MapToDto(availability, service?.Name ?? "", isBooked));
            }

            return result;
        }

        public async Task<AvailabilityDto> CreateAvailabilityAsync(Guid userId, CreateAvailabilityDto createDto)
        {
            var (_, service) = await ValidateUserAndServiceAsync(userId, createDto.ServiceId);

            var startDateTime = CalculateNextOccurrence(createDto.DayOfWeek, createDto.StartTime, createDto.StartDate);

            if (startDateTime < DateTime.UtcNow)
                throw new ArgumentException("Start time cannot be in the past");

            if (!await IsSlotAvailableAsync(userId, startDateTime, service.DurationMinutes))
                throw new InvalidOperationException("This time slot overlaps with an existing availability");

            var availability = new Availability
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ServiceId = createDto.ServiceId,
                StartTime = startDateTime,
                EndTime = startDateTime.AddMinutes(service.DurationMinutes),
                Repeat = RepeatAvailability.None,
            };

            await _availabilityRepository.AddAsync(availability);
            await _context.SaveChangesAsync();

            return MapToDto(availability, service.Name, false);
        }

        public async Task<IEnumerable<AvailabilityDto>> CreateRecurringAvailabilityAsync(Guid userId, CreateRecurringAvailabilityDto createDto)
        {
            var (_, service) = await ValidateUserAndServiceAsync(userId, createDto.ServiceId);

            if (createDto.Repeat == RepeatAvailability.None)
                throw new ArgumentException("Repeat frequency must be specified for recurring availability");

            var startDateTime = CalculateNextOccurrence(createDto.DayOfWeek, createDto.StartTime, createDto.StartDate);

            if (startDateTime < DateTime.UtcNow)
                throw new ArgumentException("Start time cannot be in the past");

            if (createDto.EndDate <= startDateTime)
                throw new ArgumentException("End date must be after start date");

            var maxEndDate = startDateTime.AddMonths(6);
            if (createDto.EndDate > maxEndDate)
                throw new ArgumentException("Cannot create recurring availabilities beyond 6 months");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var current = startDateTime;
                var createdAvailabilities = new List<AvailabilityDto>();
                const int batchSize = 100;
                int count = 0;

                while (current <= createDto.EndDate)
                {
                    if (await IsSlotAvailableAsync(userId, current, service.DurationMinutes))
                    {
                        var newAvailability = new Availability
                        {
                            Id = Guid.NewGuid(),
                            UserId = userId,
                            ServiceId = createDto.ServiceId,
                            StartTime = current,
                            EndTime = current.AddMinutes(service.DurationMinutes),
                            Repeat = createDto.Repeat,
                        };
                        
                        await _availabilityRepository.AddAsync(newAvailability);
                        createdAvailabilities.Add(MapToDto(newAvailability, service.Name, false));
                        count++;

                        if (count % batchSize == 0)
                            await _context.SaveChangesAsync();
                    }

                    current = createDto.Repeat switch
                    {
                        RepeatAvailability.Daily => current.AddDays(1),
                        RepeatAvailability.Weekly => current.AddDays(7),
                        RepeatAvailability.Monthly => current.AddMonths(1),
                        _ => throw new ArgumentException("Invalid repeat option"),
                    };

                    if (count > 1000)
                        throw new InvalidOperationException("Too many availabilities would be created. Please adjust your date range.");
                }

                if (count % batchSize != 0)
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
            var availability = await _availabilityRepository.GetByIdAsync(id);
            
            if (availability == null)
                throw new ArgumentException("Availability not found");

            if (availability.UserId != userId)
                throw new UnauthorizedAccessException("You don't have permission to update this availability");

            var service = await _serviceRepository.GetByIdAsync(availability.ServiceId);
            if (service == null)
                throw new ArgumentException("Service not found");

            DateTime newStartTime;

            if (updateDto.NewDate.HasValue)
            {
                // Use specific date with optional new time
                var timeOfDay = updateDto.StartTime ?? availability.StartTime.TimeOfDay;
                newStartTime = updateDto.NewDate.Value.Date.Add(timeOfDay);
            }
            else if (updateDto.DayOfWeek.HasValue || updateDto.StartTime.HasValue)
            {
                // Calculate new occurrence based on DayOfWeek and/or TimeSpan
                var dayOfWeek = updateDto.DayOfWeek ?? availability.StartTime.DayOfWeek;
                var timeOfDay = updateDto.StartTime ?? availability.StartTime.TimeOfDay;
                newStartTime = CalculateNextOccurrence(dayOfWeek, timeOfDay);
            }
            else
            {
                var isBooked = await IsAvailabilityBookedAsync(availability.Id);
                return MapToDto(availability, service.Name, isBooked);
            }

            if (newStartTime < DateTime.UtcNow)
                throw new ArgumentException("Start time cannot be in the past");

            if (!await IsSlotAvailableAsync(userId, newStartTime, service.DurationMinutes, id))
                throw new InvalidOperationException("This time slot overlaps with an existing availability");

            availability.StartTime = newStartTime;
            availability.EndTime = newStartTime.AddMinutes(service.DurationMinutes);

            _availabilityRepository.Update(availability);
            await _context.SaveChangesAsync();

            var isBookedAfterUpdate = await IsAvailabilityBookedAsync(availability.Id);
            return MapToDto(availability, service.Name, isBookedAfterUpdate);
        }

        public async Task DeleteAvailabilityAsync(Guid id, Guid userId)
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

        private async Task<bool> IsSlotAvailableAsync(Guid userId, DateTime startTime, int durationMinutes, Guid? excludeAvailabilityId = null)
        {
            var endTime = startTime.AddMinutes(durationMinutes);
            
            var hasOverlap = await _context.Set<Availability>()
                .AnyAsync(a => 
                    a.UserId == userId &&
                    a.Id != excludeAvailabilityId &&
                    a.StartTime < endTime &&
                    a.EndTime > startTime);

            return !hasOverlap;
        }

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

        private static DateTime CalculateNextOccurrence(DayOfWeek dayOfWeek, TimeSpan timeOfDay, DateTime? startDate = null)
        {
            var baseDate = startDate ?? DateTime.UtcNow.Date;
            var daysUntilTarget = ((int)dayOfWeek - (int)baseDate.DayOfWeek + 7) % 7;
            
            if (daysUntilTarget == 0 && baseDate.Add(timeOfDay) < DateTime.UtcNow)
                daysUntilTarget = 7;

            return baseDate.AddDays(daysUntilTarget).Date.Add(timeOfDay);
        }

        private static AvailabilityDto MapToDto(Availability availability, string serviceName, bool isBooked)
        {
            return new AvailabilityDto
            {
                Id = availability.Id,
                ServiceId = availability.ServiceId,
                StartTime = availability.StartTime,
                EndTime = availability.EndTime ?? availability.StartTime,
                ServiceName = serviceName,
                IsBooked = isBooked
            };
        }
    }
}
