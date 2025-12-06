using TurnoLink.DataAccess.Entities;

namespace TurnoLink.DataAccess.Interfaces
{
    /// <summary>
    /// Interface of the client repository
    /// </summary>
    public interface IClientRepository
    {
        /// <summary>
        /// Gets all clients
        /// </summary>
        Task<IEnumerable<Client>> GetAllAsync();

        /// <summary>
        /// Gets a client by their ID
        /// </summary>
        /// <param name="id">Client ID</param>
        Task<Client?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets a client by their email
        /// </summary>
        /// <param name="email">Client email</param>
        Task<Client?> GetByEmailAsync(string email);

        /// <summary>
        /// Gets a client by their phone number
        /// </summary>
        /// <param name="phoneNumber">Client phone number</param>
        Task<Client?> GetByPhoneAsync(string phoneNumber);

        /// <summary>
        /// Adds a new client
        /// </summary>
        /// <param name="entity">Client entity</param>
        Task<Client> AddAsync(Client entity);

        /// <summary>
        /// Updates an existing client
        /// </summary>
        /// <param name="entity">Client entity</param>
        void Update(Client entity);

        /// <summary>
        /// Deletes a client
        /// </summary>
        /// <param name="entity">Client entity</param>
        void Remove(Client entity);

        /// <summary>
        /// Checks if a client exists
        /// </summary>
        /// <param name="predicate">Condition to check</param>
        Task<bool> ExistsAsync(Func<Client, bool> predicate);
    }
}