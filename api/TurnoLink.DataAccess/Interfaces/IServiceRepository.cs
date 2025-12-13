using TurnoLink.DataAccess.Entities;

namespace TurnoLink.DataAccess.Interfaces
{
    /// <summary>
    /// Interface of the service repository
    /// </summary>
    public interface IServiceRepository
    {
        /// <summary>
        /// Gets a service by its ID
        /// </summary>
        /// <param name="id">Service ID</param>
        Task<Service?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets all services by user ID
        /// </summary>
        /// <param name="userId">User ID</param>
        Task<IEnumerable<Service>> GetServicesByUserIdAsync(Guid userId);

        /// <summary>
        /// Gets all services by slug
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        Task<IEnumerable<Service>> GetServicesBySlug(string slug);

        /// <summary>
        /// Adds a new service
        /// </summary>
        /// <param name="entity">Service entity</param>
        Task<Service> AddAsync(Service entity);

        /// <summary>
        /// Updates an existing service
        /// </summary>
        /// <param name="entity">Service entity</param>
        void Update(Service entity);

        /// <summary>
        /// Deletes a service
        /// </summary>
        /// <param name="entity">Service entity</param>
        void Remove(Service entity);

        /// <summary>
        /// Checks if a service exists
        /// </summary>
        /// <param name="predicate">Condition to check</param>
        Task<bool> ExistsAsync(Func<Service, bool> predicate);
    }
}