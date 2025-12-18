using TurnoLink.Business.DTOs;
using TurnoLink.Business.Interfaces;
using TurnoLink.DataAccess.Data;
using TurnoLink.DataAccess.Entities;
using TurnoLink.DataAccess.Interfaces;

namespace TurnoLink.Business.Services
{
    /// <summary>
    /// Service of users
    /// Implements the business logic for user operations
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly TurnoLinkDbContext _context;

        public UserService(IUserRepository userRepository, TurnoLinkDbContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                return users.Select(MapToDto);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Error retrieving all users", e);
            }
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);

                if (user == null)
                    return null;

                return MapToDto(user);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Error retrieving user by ID", e);
            }
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);

                if (user == null)
                    return null;

                return MapToDto(user);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Error retrieving user by email", e);
            }
        }

        public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                    throw new InvalidOperationException("User not found");

                // Update fields if provided
                if (!string.IsNullOrWhiteSpace(updateUserDto.Name))
                    user.Name = updateUserDto.Name;

                if (!string.IsNullOrWhiteSpace(updateUserDto.Surname))
                    user.Surname = updateUserDto.Surname;

                if (!string.IsNullOrWhiteSpace(updateUserDto.PhoneNumber))
                    user.PhoneNumber = updateUserDto.PhoneNumber;

                if (!string.IsNullOrWhiteSpace(updateUserDto.Address))
                    user.Address = updateUserDto.Address;

                if (updateUserDto.IsActive.HasValue)
                    user.IsActive = updateUserDto.IsActive.Value;

                _userRepository.Update(user);
                await _context.SaveChangesAsync();

                return MapToDto(user);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Error updating user", e);
            }
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                    throw new InvalidOperationException("User not found");

                _userRepository.Remove(user);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Error deleting user", e);
            }
        }

        /// <summary>
        /// Maps a User entity to a UserDto
        /// </summary>
        /// <param name="user">Entity of User</param>
        /// <returns>DTO of User</returns>
        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Slug = user.Slug,
                IsActive = user.IsActive
            };
        }
    }
}
