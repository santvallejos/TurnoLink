using TurnoLink.DataAccess.Entities;

namespace TurnoLink.DataAccess.Interfaces
{
    /// <sumary>
    /// Interface for availability repository
    /// </sumary>
    public interface IAvailabilityRepository
    {
        /// <sumary>
        /// Get availability entities by ID
        /// </sumary>
        /// <param name="id">Availability ID</param>
        Task<Availability?> GetByIdAsync(Guid id);

        /// <sumary>
        /// Gets availabilities by user ID
        /// </sumary>
        /// <param name="userId">User ID</param>
        Task<IEnumerable<Availability>> GetAvailabilitiesByUserIdAsync(Guid userId);

        /// <sumary>
        /// Gets availabilities by service ID
        /// </sumary>
        /// <param name="serviceId">Service ID</param>
        Task<IEnumerable<Availability>> GetAvailabilitiesByServiceIdAsync(Guid serviceId);

        /// <sumary>
        /// Adds a new availability
        /// </sumary>
        /// <param name="entity">Availability entity</param>
        Task<Availability> AddAsync(Availability entity);

        /// <sumary>
        /// Updates an existing availability
        /// </sumary>
        /// <param name="entity">Availability entity</param>
        void Update(Availability entity);

        /// <sumary>
        /// Removes an availability
        /// </sumary>
        /// <param name="entity">Availability entity</param>
        void Remove(Availability entity);

        /// <sumary>
        /// Checks if an availability exists that meets a condition
        /// </sumary>
        /// <param name="predicate">Condition to check</param>
        Task<bool> ExistsAsync(Func<Availability, bool> predicate);
    }
}