using TurnoLink.DataAccess.Entities;

namespace TurnoLink.DataAccess.Interfaces
{
    /// <summary>
    /// Interface for availability repository
    /// </summary>
    public interface IAvailabilityRepository
    {
        /// <summary>
        /// Get availability entities by ID
        /// </summary>
        /// <param name="id">Availability ID</param>
        Task<Availability?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets availabilities by user ID
        /// </summary>
        /// <param name="userId">User ID</param>
        Task<IEnumerable<Availability>> GetAvailabilitiesByUserIdAsync(Guid userId);

        /// <summary>
        /// Gets availabilities by service ID
        /// </summary>
        /// <param name="serviceId">Service ID</param>
        Task<IEnumerable<Availability>> GetAvailabilitiesByServiceIdAsync(Guid serviceId);

        /// <summary>
        /// Adds a new availability
        /// </summary>
        /// <param name="entity">Availability entity</param>
        Task<Availability> AddAsync(Availability entity);

        /// <summary>
        /// Updates an existing availability
        /// </summary>
        /// <param name="entity">Availability entity</param>
        void Update(Availability entity);

        /// <summary>
        /// Removes an availability
        /// </summary>
        /// <param name="entity">Availability entity</param>
        void Remove(Availability entity);

        /// <summary>
        /// Checks if an availability exists that meets a condition
        /// </summary>
        /// <param name="predicate">Condition to check</param>
        Task<bool> ExistsAsync(Func<Availability, bool> predicate);
    }
}