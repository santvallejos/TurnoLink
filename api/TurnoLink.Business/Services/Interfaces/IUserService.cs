using TurnoLink.Business.DTOs;

namespace TurnoLink.Business.Services.Interfaces;

/// <summary>
/// Interfaz para el servicio de gestión de usuarios
/// </summary>
public interface IUserService
{
    Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserDto createUserDto);
    Task<ApiResponse<UserDto>> GetUserByIdAsync(Guid id);
    Task<ApiResponse<UserDto>> GetUserByEmailAsync(string email);
    Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync();
    Task<ApiResponse<IEnumerable<UserDto>>> GetActiveUsersAsync();
    Task<ApiResponse<UserDto>> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto);
    Task<ApiResponse<bool>> DeleteUserAsync(Guid id);
}
