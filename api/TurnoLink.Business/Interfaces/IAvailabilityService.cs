using TurnoLink.Business.DTOs;

namespace TurnoLink.Business.Interfaces
{
    /// <summary>
    /// Interface for availability business logic.
    /// All times are handled in UTC format.
    /// </summary>
    public interface IAvailabilityService
    {
        /// <summary>
        /// Gets an availability by ID
        /// </summary>
        /// <param name="id">Availability ID</param>
        /// <returns>Availability DTO or null if not found</returns>
        Task<AvailabilityDto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets all future availabilities for a specific user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>Collection of availability DTOs ordered by start time</returns>
        Task<IEnumerable<AvailabilityDto>> GetAvailabilitiesByUserIdAsync(Guid userId);

        /// <summary>
        /// Gets all future availabilities for a specific service
        /// </summary>
        /// <param name="serviceId">Service ID</param>
        /// <returns>Collection of availability DTOs ordered by start time</returns>
        Task<IEnumerable<AvailabilityDto>> GetAvailabilitiesByServiceIdAsync(Guid serviceId);

        /// <summary>
        /// Gets available slots for a user within a UTC date range
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="startDateUtc">Start date in UTC</param>
        /// <param name="endDateUtc">End date in UTC</param>
        /// <returns>Collection of available slots</returns>
        Task<IEnumerable<AvailabilityDto>> GetAvailableSlotsByDateRangeAsync(Guid userId, DateTime startDateUtc, DateTime endDateUtc);

        /// <summary>
        /// Creates a single availability slot.
        /// The frontend sends local date/time with UTC offset, and the backend converts to UTC.
        /// End time is automatically calculated based on service duration.
        /// </summary>
        /// <param name="userId">User ID creating the availability</param>
        /// <param name="createDto">Availability creation data with local date, time, and UTC offset</param>
        /// <returns>Created availability DTO with times in UTC</returns>
        Task<AvailabilityDto> CreateAvailabilityAsync(Guid userId, CreateAvailabilityDto createDto);

        /// <summary>
        /// Creates recurring availability slots from start date to end date.
        /// All slots will have the same time (converted to UTC) based on repeat frequency.
        /// </summary>
        /// <param name="userId">User ID creating the availability</param>
        /// <param name="createDto">Recurring availability creation data</param>
        /// <returns>Collection of created availability DTOs</returns>
        Task<IEnumerable<AvailabilityDto>> CreateRecurringAvailabilityAsync(Guid userId, CreateRecurringAvailabilityDto createDto);

        /// <summary>
        /// Updates an existing availability's date and/or time
        /// </summary>
        /// <param name="id">Availability ID</param>
        /// <param name="userId">User ID performing the update</param>
        /// <param name="updateDto">Update data with optional new date/time and required UTC offset</param>
        /// <returns>Updated availability DTO</returns>
        Task<AvailabilityDto> UpdateAvailabilityAsync(Guid id, Guid userId, UpdateAvailabilityDto updateDto);

        /// <summary>
        /// Deletes an availability slot if it has no active bookings
        /// </summary>
        /// <param name="id">Availability ID</param>
        /// <param name="userId">User ID performing the deletion</param>
        Task DeleteAvailabilityAsync(Guid id, Guid userId);
    }
}
