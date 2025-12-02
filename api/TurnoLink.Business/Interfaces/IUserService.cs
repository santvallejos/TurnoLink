using TurnoLink.Business.DTOs;

namespace TurnoLink.Business.Interfaces
{
    /// <summary>
    /// Interfaz para operaciones relacionadas con usuarios
    /// </summary>
    public interface IUserService
    {
        Task<UserDto?> GetUserByIdAsync(Guid id);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<IEnumerable<UserDto>> GetActiveUsersAsync();
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto);
        Task<bool> DeleteUserAsync(Guid id);
    }
}