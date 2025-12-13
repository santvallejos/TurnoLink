using TurnoLink.Business.DTOs;
using TurnoLink.DataAccess.Entities;

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
        /// <param name="userId"></param>
        /// <returns></returns>
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
        /// Creates a new availability without recurring 
        /// </summary>
        /// <param name="userId">User ID creating the availability</param>
        /// <param name="createDto">Availability creation data</param>
        /// <returns>Created availability DTO</returns>
        public Task<AvailabilityDto> CreateAvailabilityNotRecurringAsync(Guid userId, CreateAvailabilityDto createDto);

        /// <summary>
        /// Creates a new availability with recurring options
        /// </summary>
        /// <param name="userId">User ID creating the availability</param>
        /// <param name="createDto">Availability creation data</param>
        /// <returns>Created availability DTO</returns>
        Task<IEnumerable<AvailabilityDto>> CreateAvailabilityWhitRecurringAsync(Guid userId, CreateRecurringAvailabilityDto createDto);

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
