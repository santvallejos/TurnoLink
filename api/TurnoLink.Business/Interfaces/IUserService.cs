using TurnoLink.Business.DTOs;

namespace TurnoLink.Business.Interfaces
{
    /// <summary>
    /// Interface for user-related operations.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Get all users.
        /// </summary>
        Task<IEnumerable<UserDto>> GetAllUsersAsync();

        /// <summary>
        /// Get a user by their unique identifier.
        /// </summary>
        /// <param name="id">User ID</param>
        Task<UserDto?> GetUserByIdAsync(Guid id);

        /// <summary>
        /// Get a user by their email address.
        /// </summary>
        /// <param name="email">User email</param>
        Task<UserDto?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Update an existing user.
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="updateUserDto">DTO containing user update data</param>
        Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto);

        /// <summary>
        /// Delete a user by their unique identifier.
        /// </summary>
        /// <param name="id">User ID</param>
        Task<bool> DeleteUserAsync(Guid id);
    }
}