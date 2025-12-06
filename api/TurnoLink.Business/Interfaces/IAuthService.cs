using TurnoLink.Business.DTOs;

namespace TurnoLink.Business.Interfaces
{
    /// <summary>
    /// Interface for authentication-related operations.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user in the system
        /// </summary>
        /// <param name="registerDto">DTO containing registration data</param>
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);

        /// <summary>
        /// Authenticates a user and generates a JWT token
        /// </summary>
        /// <param name="loginDto">DTO containing login data</param>
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);

        /// <summary>
        /// Validates a JWT token
        /// </summary>
        /// <param name="token">JWT token</param>
        Task<bool> ValidateTokenAsync(string token);
    }
}
