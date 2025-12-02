using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurnoLink.Business.DTOs;
using TurnoLink.Business.Interfaces;

namespace TurnoLink.WebAPI.Controllers
{
    /// <summary>
    /// Controlador para gestión de usuarios profesionales
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize] // Requiere autenticación para todos los endpoints
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los usuarios
        /// </summary>
        /// <returns>Lista de usuarios</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            _logger.LogInformation("Obteniendo todos los usuarios");
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Obtiene usuarios activos
        /// </summary>
        /// <returns>Lista de usuarios activos</returns>
        [HttpGet("active")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetActiveUsers()
        {
            _logger.LogInformation("Obteniendo usuarios activos");
            var users = await _userService.GetActiveUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Obtiene un usuario por su ID
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <returns>Usuario encontrado</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUserById(Guid id)
        {
            _logger.LogInformation("Obteniendo usuario con ID: {UserId}", id);
            var user = await _userService.GetUserByIdAsync(id);
            
            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            return Ok(user);
        }

        /// <summary>
        /// Obtiene un usuario por su email
        /// </summary>
        /// <param name="email">Email del usuario</param>
        /// <returns>Usuario encontrado</returns>
        [HttpGet("email/{email}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUserByEmail(string email)
        {
            _logger.LogInformation("Obteniendo usuario con email: {Email}", email);
            var user = await _userService.GetUserByEmailAsync(email);
            
            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            return Ok(user);
        }

        /// <summary>
        /// Crea un nuevo usuario
        /// </summary>
        /// <param name="createUserDto">Datos del nuevo usuario</param>
        /// <returns>Usuario creado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                _logger.LogInformation("Creando nuevo usuario con email: {Email}", createUserDto.Email);
                var user = await _userService.CreateUserAsync(createUserDto);
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un usuario existente
        /// </summary>
        /// <param name="id">ID del usuario a actualizar</param>
        /// <param name="updateUserDto">Datos actualizados</param>
        /// <returns>Usuario actualizado</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> UpdateUser(Guid id, [FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                _logger.LogInformation("Actualizando usuario con ID: {UserId}", id);
                var user = await _userService.UpdateUserAsync(id, updateUserDto);
                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un usuario
        /// </summary>
        /// <param name="id">ID del usuario a eliminar</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            try
            {
                _logger.LogInformation("Eliminando usuario con ID: {UserId}", id);
                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
