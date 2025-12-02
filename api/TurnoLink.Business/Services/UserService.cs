using TurnoLink.Business.DTOs;
using TurnoLink.Business.Services.Interfaces;
using TurnoLink.DataAccess.Entities;
using TurnoLink.DataAccess.Repositories.Interfaces;

namespace TurnoLink.Business.Services;

/// <summary>
/// Servicio de gestión de usuarios
/// Implementa la lógica de negocio para operaciones con usuarios
/// </summary>
public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserDto createUserDto)
    {
        try
        {
            // Validar que el email no exista
            var existingUser = await _unitOfWork.Users.GetByEmailAsync(createUserDto.Email);
            if (existingUser != null)
            {
                return ApiResponse<UserDto>.ErrorResult("El email ya está registrado");
            }

            // TODO: Implementar hash de contraseña
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = createUserDto.FullName,
                Email = createUserDto.Email,
                PasswordHash = passwordHash,
                PhoneNumber = createUserDto.PhoneNumber,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var userDto = MapToDto(user);
            return ApiResponse<UserDto>.SuccessResult(userDto, "Usuario creado exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.ErrorResult($"Error al crear usuario: {ex.Message}");
        }
    }

    public async Task<ApiResponse<UserDto>> GetUserByIdAsync(Guid id)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                return ApiResponse<UserDto>.ErrorResult("Usuario no encontrado");
            }

            var userDto = MapToDto(user);
            return ApiResponse<UserDto>.SuccessResult(userDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.ErrorResult($"Error al obtener usuario: {ex.Message}");
        }
    }

    public async Task<ApiResponse<UserDto>> GetUserByEmailAsync(string email)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user == null)
            {
                return ApiResponse<UserDto>.ErrorResult("Usuario no encontrado");
            }

            var userDto = MapToDto(user);
            return ApiResponse<UserDto>.SuccessResult(userDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.ErrorResult($"Error al obtener usuario: {ex.Message}");
        }
    }

    public async Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync()
    {
        try
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            var userDtos = users.Select(MapToDto);
            return ApiResponse<IEnumerable<UserDto>>.SuccessResult(userDtos);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<UserDto>>.ErrorResult($"Error al obtener usuarios: {ex.Message}");
        }
    }

    public async Task<ApiResponse<IEnumerable<UserDto>>> GetActiveUsersAsync()
    {
        try
        {
            var users = await _unitOfWork.Users.GetActiveUsersAsync();
            var userDtos = users.Select(MapToDto);
            return ApiResponse<IEnumerable<UserDto>>.SuccessResult(userDtos);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<UserDto>>.ErrorResult($"Error al obtener usuarios activos: {ex.Message}");
        }
    }

    public async Task<ApiResponse<UserDto>> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                return ApiResponse<UserDto>.ErrorResult("Usuario no encontrado");
            }

            if (!string.IsNullOrWhiteSpace(updateUserDto.FullName))
                user.FullName = updateUserDto.FullName;

            if (!string.IsNullOrWhiteSpace(updateUserDto.PhoneNumber))
                user.PhoneNumber = updateUserDto.PhoneNumber;

            if (updateUserDto.IsActive.HasValue)
                user.IsActive = updateUserDto.IsActive.Value;

            user.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            var userDto = MapToDto(user);
            return ApiResponse<UserDto>.SuccessResult(userDto, "Usuario actualizado exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.ErrorResult($"Error al actualizar usuario: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteUserAsync(Guid id)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                return ApiResponse<bool>.ErrorResult("Usuario no encontrado");
            }

            _unitOfWork.Users.Remove(user);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResult(true, "Usuario eliminado exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResult($"Error al eliminar usuario: {ex.Message}");
        }
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };
    }
}
