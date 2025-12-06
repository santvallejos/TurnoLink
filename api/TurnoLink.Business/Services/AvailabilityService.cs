using TurnoLink.Business.DTOs;
using TurnoLink.Business.Interfaces;
using TurnoLink.DataAccess.Entities;
using TurnoLink.DataAccess.Enums;
using TurnoLink.DataAccess.Interfaces;

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

        /// <summary>
        /// Constructor for AvailabilityService.
        /// </summary>
        /// <param name="availabilityRepository">Availability repository</param>
        /// <param name="serviceRepository">Service repository</param>
        /// <param name="userRepository">User repository</param>
        /// <param name="bookingRepository">Booking repository</param>
        public AvailabilityService(
            IAvailabilityRepository availabilityRepository,
            IServiceRepository serviceRepository,
            IUserRepository userRepository,
            IBookingRepository bookingRepository)
        {
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

        /// <summary>
        /// Creates a new availability slot
        /// </summary>
        public async Task<AvailabilityDto> CreateAvailabilityAsync(Guid userId, CreateAvailabilityDto createDto)
        {
            // Validar que el usuario existe
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("Usuario no encontrado");

            // Validar que el servicio existe y pertenece al usuario
            var service = await _serviceRepository.GetByIdAsync(createDto.ServiceId);
            if (service == null)
                throw new ArgumentException("Servicio no encontrado");

            if (service.UserId != userId)
                throw new UnauthorizedAccessException("No tiene permiso para crear disponibilidad para este servicio");

            // Validar que la fecha es futura
            if (createDto.StartTime <= DateTime.UtcNow)
                throw new ArgumentException("La fecha de inicio debe ser futura");

            // Validar que no hay solapamiento
            var isAvailable = await IsSlotAvailableAsync(userId, createDto.StartTime, createDto.DurationMinutes);
            if (!isAvailable)
                throw new InvalidOperationException("El slot de tiempo se solapa con otra disponibilidad o reserva existente");

            var availability = new Availability
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ServiceId = createDto.ServiceId,
                StartTime = createDto.StartTime,
                DurationMinutes = createDto.DurationMinutes
            };

            var created = await _availabilityRepository.AddAsync(availability);
            return MapToDto(created, service.Name);
        }

        /// <summary>
        /// Updates an existing availability
        /// </summary>
        public async Task<AvailabilityDto> UpdateAvailabilityAsync(Guid id, Guid userId, UpdateAvailabilityDto updateDto)
        {
            var availability = await _availabilityRepository.GetByIdAsync(id);
            if (availability == null)
                throw new ArgumentException("Disponibilidad no encontrada");

            if (availability.UserId != userId)
                throw new UnauthorizedAccessException("No tiene permiso para modificar esta disponibilidad");

            // Validar si hay cambios en tiempo
            if (updateDto.StartTime.HasValue || updateDto.DurationMinutes.HasValue)
            {
                var newStartTime = updateDto.StartTime ?? availability.StartTime;
                var newDuration = updateDto.DurationMinutes ?? availability.DurationMinutes;

                // Validar que la fecha sigue siendo futura
                if (newStartTime <= DateTime.UtcNow)
                    throw new ArgumentException("La fecha de inicio debe ser futura");

                // Validar que no hay solapamiento (excluyendo esta misma disponibilidad)
                var isAvailable = await IsSlotAvailableAsync(userId, newStartTime, newDuration, id);
                if (!isAvailable)
                    throw new InvalidOperationException("El slot de tiempo se solapa con otra disponibilidad o reserva existente");

                availability.StartTime = newStartTime;
                availability.DurationMinutes = newDuration;
            }

            _availabilityRepository.Update(availability);

            var service = await _serviceRepository.GetByIdAsync(availability.ServiceId);
            return MapToDto(availability, service?.Name ?? "");
        }

        /// <summary>
        /// Deletes an availability slot
        /// </summary>
        public async Task DeleteAvailabilityAsync(Guid id, Guid userId)
        {
            var availability = await _availabilityRepository.GetByIdAsync(id);
            if (availability == null)
                throw new ArgumentException("Disponibilidad no encontrada");

            if (availability.UserId != userId)
                throw new UnauthorizedAccessException("No tiene permiso para eliminar esta disponibilidad");

            // Validar que no hay reservas asociadas a este slot
            var bookings = await _bookingRepository.GetBookingsByUserIdAsync(userId);
            var hasBookings = bookings.Any(b => 
                b.StartTime >= availability.StartTime && 
                b.StartTime < availability.StartTime.AddMinutes(availability.DurationMinutes));

            if (hasBookings)
                throw new InvalidOperationException("No se puede eliminar una disponibilidad que tiene reservas asociadas");

            _availabilityRepository.Remove(availability);
        }

        /// <summary>
        /// Checks if a time slot is available (no overlap with other availabilities or bookings)
        /// </summary>
        public async Task<bool> IsSlotAvailableAsync(
            Guid userId, 
            DateTime startTime, 
            int durationMinutes, 
            Guid? excludeAvailabilityId = null)
        {
            var endTime = startTime.AddMinutes(durationMinutes);

            // Verificar solapamiento con otras disponibilidades
            var availabilities = await _availabilityRepository.GetAvailabilitiesByUserIdAsync(userId);
            var overlappingAvailabilities = availabilities
                .Where(a => excludeAvailabilityId == null || a.Id != excludeAvailabilityId)
                .Where(a =>
                {
                    var aEndTime = a.StartTime.AddMinutes(a.DurationMinutes);
                    return (startTime < aEndTime && endTime > a.StartTime);
                });

            if (overlappingAvailabilities.Any())
                return false;

            // Verificar solapamiento con reservas existentes
            var bookings = await _bookingRepository.GetBookingsByUserIdAsync(userId);
            var overlappingBookings = bookings
                .Where(b => b.Status != BookingStatus.Canceled)
                .Where(b => (startTime < b.EndTime && endTime > b.StartTime));

            return !overlappingBookings.Any();
        }

        /// <summary>
        /// Maps Availability entity to DTO
        /// </summary>
        private AvailabilityDto MapToDto(Availability availability, string serviceName)
        {
            return new AvailabilityDto
            {
                Id = availability.Id,
                UserId = availability.UserId,
                ServiceId = availability.ServiceId,
                StartTime = availability.StartTime,
                EndTime = availability.StartTime.AddMinutes(availability.DurationMinutes),
                DurationMinutes = availability.DurationMinutes,
                ServiceName = serviceName
            };
        }
    }
}
