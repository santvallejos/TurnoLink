using TurnoLink.Business.DTOs;
using TurnoLink.Business.Interfaces;
using TurnoLink.DataAccess.Data;
using TurnoLink.DataAccess.Entities;
using TurnoLink.DataAccess.Interfaces;

namespace TurnoLink.Business.Services
{
    /// <summary>
    /// Servicio de gestión de usuarios
    /// Implementa la lógica de negocio para operaciones con usuarios
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

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            // Validar que el email no exista
            var existingUser = await _userRepository.GetByEmailAsync(createUserDto.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("El email ya está registrado");
            }

            // Hash de contraseña
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

            await _userRepository.AddAsync(user);
            await _context.SaveChangesAsync();

            return MapToDto(user);
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : MapToDto(user);
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user == null ? null : MapToDto(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToDto);
        }

        public async Task<IEnumerable<UserDto>> GetActiveUsersAsync()
        {
            var users = await _userRepository.GetActiveUsersAsync();
            return users.Select(MapToDto);
        }

        public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new InvalidOperationException("Usuario no encontrado");
            }

            if (!string.IsNullOrWhiteSpace(updateUserDto.FullName))
                user.FullName = updateUserDto.FullName;

            if (!string.IsNullOrWhiteSpace(updateUserDto.PhoneNumber))
                user.PhoneNumber = updateUserDto.PhoneNumber;

            if (updateUserDto.IsActive.HasValue)
                user.IsActive = updateUserDto.IsActive.Value;

            user.UpdatedAt = DateTime.UtcNow;

            _userRepository.Update(user);
            await _context.SaveChangesAsync();

            return MapToDto(user);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new InvalidOperationException("Usuario no encontrado");
            }

            _userRepository.Remove(user);
            await _context.SaveChangesAsync();

            return true;
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
}
