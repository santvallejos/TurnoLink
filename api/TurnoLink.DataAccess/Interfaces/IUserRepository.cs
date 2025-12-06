using TurnoLink.DataAccess.Entities;

namespace TurnoLink.DataAccess.Interfaces
{
    /// <summary>
    /// Interface of the user repository
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Gets all user entities
        /// </summary>
        Task<IEnumerable<User>> GetAllAsync();

        /// <summary>
        /// Gets a user by their ID
        /// </summary>
        /// <param name="id">User ID</param>
        Task<User?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets a user by their email
        /// </summary>
        /// <param name="email">User email</param>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Gets active users
        /// </summary>
        Task<IEnumerable<User>> GetActiveUsersAsync();

        /// <summary>
        /// Adds a new user
        /// </summary>
        /// <param name="entity">User entity</param>
        Task<User> AddAsync(User entity);

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="entity">User entity</param>
        void Update(User entity);

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="entity">User entity</param>
        void Remove(User entity);

        /// <summary>
        /// Checks if a user exists that meets a condition
        /// </summary>
        /// <param name="predicate">Condition to check</param>
        Task<bool> ExistsAsync(Func<User, bool> predicate);
    }
}
