using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurnoLink.Business.DTOs;
using TurnoLink.Business.Services.Interfaces;

namespace TurnoLink.WebAPI.Controllers;

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
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register([FromBody] RegisterDto registerDto)
    {
        _logger.LogInformation("Intentando registrar usuario con email: {Email}", registerDto.Email);
        
        var response = await _authService.RegisterAsync(registerDto);
        
        if (!response.Success)
        {
            _logger.LogWarning("Error en registro: {Message}", response.Message);
            return BadRequest(response);
        }

        _logger.LogInformation("Usuario registrado exitosamente: {Email}", registerDto.Email);
        return Ok(response);
    }

    /// <summary>
    /// Autentica un usuario y genera un token JWT
    /// </summary>
    /// <param name="loginDto">Credenciales de login</param>
    /// <returns>Token de autenticación y datos del usuario</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginDto loginDto)
    {
        _logger.LogInformation("Intento de login para: {Email}", loginDto.Email);
        
        var response = await _authService.LoginAsync(loginDto);
        
        if (!response.Success)
        {
            _logger.LogWarning("Login fallido para: {Email}", loginDto.Email);
            return Unauthorized(response);
        }

        _logger.LogInformation("Login exitoso para: {Email}", loginDto.Email);
        return Ok(response);
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
