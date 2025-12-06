using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurnoLink.Business.DTOs;
using TurnoLink.Business.Interfaces;

namespace TurnoLink.WebAPI.Controllers
{
    /// <summary>
    /// Controller for handling authentication-related endpoints
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
        /// Register a new user and generate a JWT token
        /// </summary>
        /// <param name="registerDto">Registration data</param>
        /// <returns>Authentication token and user data</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                _logger.LogInformation("Attempting to register user with email: {Email}", registerDto.Email);
                var response = await _authService.RegisterAsync(registerDto);
                _logger.LogInformation("User registered successfully: {Email}", registerDto.Email);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Registration error: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Authenticates a user and generates a JWT token
        /// </summary>
        /// <param name="loginDto">Login credentials</param>
        /// <returns>Authentication token and user data</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                _logger.LogInformation("Attempting login for: {Email}", loginDto.Email);
                var response = await _authService.LoginAsync(loginDto);
                _logger.LogInformation("Successful login for: {Email}", loginDto.Email);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Failed login for: {Email}", loginDto.Email);
                return Unauthorized(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Test endpoint to verify authentication
        /// Requires a valid JWT token
        /// </summary>
        /// <returns>Authenticated user information</returns>
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
                message = "User is authenticated"
            });
        }
    }
}
