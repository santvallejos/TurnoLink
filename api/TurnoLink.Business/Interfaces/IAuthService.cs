using TurnoLink.Business.DTOs;

namespace TurnoLink.Business.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de autenticación
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registra un nuevo usuario en el sistema
        /// </summary>
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);

        /// <summary>
        /// Autentica un usuario y genera un token JWT
        /// </summary>
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);

        /// <summary>
        /// Valida un token JWT
        /// </summary>
        Task<bool> ValidateTokenAsync(string token);
    }
}
