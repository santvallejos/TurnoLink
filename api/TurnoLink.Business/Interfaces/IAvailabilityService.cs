using TurnoLink.Business.DTOs;

namespace TurnoLink.Business.Interfaces
{
    /// <summary>
    /// Interface for availability business logic
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
        /// Gets all availabilities for a specific user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>Collection of availability DTOs</returns>
        Task<IEnumerable<AvailabilityDto>> GetAvailabilitiesByUserIdAsync(Guid userId);

        /// <summary>
        /// Gets all availabilities for a specific service
        /// </summary>
        /// <param name="serviceId">Service ID</param>
        /// <returns>Collection of availability DTOs</returns>
        Task<IEnumerable<AvailabilityDto>> GetAvailabilitiesByServiceIdAsync(Guid serviceId);

        /// <summary>
        /// Gets available slots for a user within a date range
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>Collection of available slots</returns>
        Task<IEnumerable<AvailabilityDto>> GetAvailableSlotsByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Creates a new availability using DayOfWeek and TimeSpan
        /// </summary>
        /// <param name="userId">User ID creating the availability</param>
        /// <param name="createDto">Availability creation data with DayOfWeek and TimeSpan</param>
        /// <returns>Created availability DTO</returns>
        Task<AvailabilityDto> CreateAvailabilityAsync(Guid userId, CreateAvailabilityDto createDto);

        /// <summary>
        /// Creates recurring availability using DayOfWeek and TimeSpan
        /// </summary>
        /// <param name="userId">User ID creating the availability</param>
        /// <param name="createDto">Recurring availability creation data</param>
        /// <returns>Collection of created availability DTOs</returns>
        Task<IEnumerable<AvailabilityDto>> CreateRecurringAvailabilityAsync(Guid userId, CreateRecurringAvailabilityDto createDto);

        /// <summary>
        /// Updates an existing availability
        /// </summary>
        /// <param name="id">Availability ID</param>
        /// <param name="userId">User ID performing the update</param>
        /// <param name="updateDto">Update data</param>
        /// <returns>Updated availability DTO</returns>
        Task<AvailabilityDto> UpdateAvailabilityAsync(Guid id, Guid userId, UpdateAvailabilityDto updateDto);

        /// <summary>
        /// Deletes an availability slot
        /// </summary>
        /// <param name="id">Availability ID</param>
        /// <param name="userId">User ID performing the deletion</param>
        Task DeleteAvailabilityAsync(Guid id, Guid userId);
    }
}
