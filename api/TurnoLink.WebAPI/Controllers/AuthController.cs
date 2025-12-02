using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurnoLink.Business.DTOs;
using TurnoLink.Business.Interfaces;

namespace TurnoLink.WebAPI.Controllers
{
    /// <summary>
    /// Controlador para autenticación y registro de usuarios
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Registra un nuevo usuario en el sistema
        /// </summary>
        /// <param name="registerDto">Datos de registro</param>
        /// <returns>Token de autenticación y datos del usuario</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                _logger.LogInformation("Intentando registrar usuario con email: {Email}", registerDto.Email);
                var response = await _authService.RegisterAsync(registerDto);
                _logger.LogInformation("Usuario registrado exitosamente: {Email}", registerDto.Email);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Error en registro: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Autentica un usuario y genera un token JWT
        /// </summary>
        /// <param name="loginDto">Credenciales de login</param>
        /// <returns>Token de autenticación y datos del usuario</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                _logger.LogInformation("Intento de login para: {Email}", loginDto.Email);
                var response = await _authService.LoginAsync(loginDto);
                _logger.LogInformation("Login exitoso para: {Email}", loginDto.Email);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Login fallido para: {Email}", loginDto.Email);
                return Unauthorized(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Endpoint de prueba para verificar autenticación
        /// Requiere token JWT válido
        /// </summary>
        /// <returns>Información del usuario autenticado</returns>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<object> GetCurrentUser()
        {
            var userId = User.FindFirst("userId")?.Value;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var name = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

            return Ok(new
            {
                userId,
                email,
                name,
                message = "Usuario autenticado correctamente"
            });
        }
    }
}
