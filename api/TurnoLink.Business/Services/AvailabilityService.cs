using System.Threading.Tasks;
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
        private readonly IBookingRepository _bookingRepository;
        private readonly TurnoLinkDbContext _context;

        /// <summary>
        /// Constructor for AvailabilityService.
        /// </summary>
        /// <param name="availabilityRepository">Availability repository</param>
        /// <param name="serviceRepository">Service repository</param>
        /// <param name="userRepository">User repository</param>
        /// <param name="bookingRepository">Booking repository</param>
        /// <param name="context">Database context</param>
        public AvailabilityService(
            IAvailabilityRepository availabilityRepository,
            IServiceRepository serviceRepository,
            IUserRepository userRepository,
            IBookingRepository bookingRepository,
            TurnoLinkDbContext context)
        {
            _context = context;
            _availabilityRepository = availabilityRepository;
            _serviceRepository = serviceRepository;
            _userRepository = userRepository;
            _bookingRepository = bookingRepository;
        }

        /// <summary>
        /// Gets an availability by ID
        /// </summary>
        public async Task<AvailabilityDto?> GetByIdAsync(Guid id)
        {
            var availability = await _availabilityRepository.GetByIdAsync(id);
            if (availability == null)
                throw new ArgumentException("Availability not found");

            var service = await _serviceRepository.GetByIdAsync(availability.ServiceId);

            return MapToDto(availability, service?.Name ?? "");
        }

        /// <summary>
        /// Gets all availabilities for a specific user
        /// </summary>
        public async Task<IEnumerable<AvailabilityDto>> GetAvailabilitiesByUserIdAsync(Guid userId)
        {
            var availabilities = await _availabilityRepository.GetAvailabilitiesByUserIdAsync(userId);
            var result = new List<AvailabilityDto>();

            foreach (var availability in availabilities)
            {
                var service = await _serviceRepository.GetByIdAsync(availability.ServiceId);
                result.Add(MapToDto(availability, service?.Name ?? ""));
            }

            return result;
        }

        /// <summary>
        /// Gets all availabilities for a specific service
        /// </summary>
        public async Task<IEnumerable<AvailabilityDto>> GetAvailabilitiesByServiceIdAsync(Guid serviceId)
        {
            var availabilities = await _availabilityRepository.GetAvailabilitiesByServiceIdAsync(serviceId);
            var service = await _serviceRepository.GetByIdAsync(serviceId);
            var serviceName = service?.Name ?? "";

            return availabilities.Select(a => MapToDto(a, serviceName));
        }

        /// <summary>
        /// Gets available slots for a user within a date range
        /// </summary>
        public async Task<IEnumerable<AvailabilityDto>> GetAvailableSlotsByDateRangeAsync(
            Guid userId, 
            DateTime startDate, 
            DateTime endDate)
        {
            var availabilities = await _availabilityRepository.GetAvailabilitiesByUserIdAsync(userId);
            
            var filtered = availabilities
                .Where(a => a.StartTime >= startDate && a.StartTime <= endDate)
                .OrderBy(a => a.StartTime);

            var result = new List<AvailabilityDto>();
            foreach (var availability in filtered)
            {
                var service = await _serviceRepository.GetByIdAsync(availability.ServiceId);
                result.Add(MapToDto(availability, service?.Name ?? ""));
            }

            return result;
        }

        public async Task<IEnumerable<AvailabilityDto>> CreateAvailabilityAsync(Guid UserId, CreateAvailabilityDto createAvailability)
        {
            var user = await _userRepository.GetByIdAsync(UserId);
            if (user == null)
                throw new ArgumentException("User not found");

            var service = await _serviceRepository.GetByIdAsync(createAvailability.ServiceId);
            if (service == null || service.UserId != UserId)
                throw new ArgumentException("Service not found or does not belong to user");

            if (createAvailability.StartTime < DateTime.UtcNow)
                throw new ArgumentException("Start time cannot be in the past");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                List<AvailabilityDto> createdAvailabilities;

                if (createAvailability.Repeat == RepeatAvailability.None)
                {
                    var availability = new Availability
                    {
                        Id = Guid.NewGuid(),
                        UserId = UserId,
                        ServiceId = createAvailability.ServiceId,
                        StartTime = createAvailability.StartTime,
                        EndTime = createAvailability.StartTime.AddMinutes(service.DurationMinutes),
                        Repeat = createAvailability.Repeat,
                    };
                    await _availabilityRepository.AddAsync(availability);
                    await _context.SaveChangesAsync();

                    createdAvailabilities = new List<AvailabilityDto> { MapToDto(availability, service.Name) };
                }
                else
                {
                    if (createAvailability.EndTime == null)
                        throw new ArgumentException("End time is required for recurring availabilities");

                    var endTime = createAvailability.EndTime.Value;
                    
                    if (endTime <= createAvailability.StartTime)
                        throw new ArgumentException("End time must be after start time");

                    var maxEndDate = createAvailability.StartTime.AddMonths(6);
                    if (endTime > maxEndDate)
                        throw new ArgumentException("Cannot create recurring availabilities beyond 6 months");

                    var current = createAvailability.StartTime;
                    createdAvailabilities = new List<AvailabilityDto>();
                    const int batchSize = 100;
                    int count = 0;

                    while (current <= endTime)
                    {
                        var newAvailability = new Availability
                        {
                            Id = Guid.NewGuid(),
                            UserId = UserId,
                            ServiceId = createAvailability.ServiceId,
                            StartTime = current,
                            EndTime = current.AddMinutes(service.DurationMinutes),
                            Repeat = createAvailability.Repeat,
                        };
                        await _availabilityRepository.AddAsync(newAvailability);
                        createdAvailabilities.Add(MapToDto(newAvailability, service.Name));

                        count++;
                        if (count % batchSize == 0)
                        {
                            await _context.SaveChangesAsync();
                        }

                        current = createAvailability.Repeat switch
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
                    {
                        await _context.SaveChangesAsync();
                    }
                }

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
            
            if (updateDto.StartTime != null)
            {
                if (updateDto.StartTime < DateTime.UtcNow)
                    throw new ArgumentException("Start time cannot be in the past");

                availability.StartTime = updateDto.StartTime.Value;
                var service = await _serviceRepository.GetByIdAsync(availability.ServiceId);
                if (service != null)
                {
                    availability.EndTime = availability.StartTime.AddMinutes(service.DurationMinutes);
                }
            }

            _availabilityRepository.Update(availability);
            await _context.SaveChangesAsync();
            var updatedService = await _serviceRepository.GetByIdAsync(availability.ServiceId);
            return MapToDto(availability, updatedService?.Name ?? "");
        }

        public async Task DeleteAvailabilityAsync(Guid availabilityId, Guid userId)
        {
            var availability = await _availabilityRepository.GetByIdAsync(availabilityId);
            
            if (availability == null)
                throw new ArgumentException("Availability not found");

            if (availability.UserId != userId)
                throw new UnauthorizedAccessException("You don't have permission to delete this availability");

            var hasBookings = await _context.Set<Booking>()
                .AnyAsync(b => b.AvailabilityId == availabilityId && b.Status != BookingStatus.Canceled);

            if (hasBookings)
                throw new InvalidOperationException("Cannot delete availability with active bookings");

            _availabilityRepository.Remove(availability);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Maps Availability entity to DTO
        /// </summary>
        private AvailabilityDto MapToDto(Availability availability, string serviceName)
        {
            return new AvailabilityDto
            {
                Id = availability.Id,
                ServiceId = availability.ServiceId,
                StartTime = availability.StartTime,
                EndTime = availability.EndTime ?? availability.StartTime,
                ServiceName = serviceName
            };
        }
    }
}
