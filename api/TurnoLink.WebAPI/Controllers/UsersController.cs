using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurnoLink.Business.DTOs;
using TurnoLink.Business.Services.Interfaces;

namespace TurnoLink.WebAPI.Controllers;

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
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetAllUsers()
    {
        _logger.LogInformation("Obteniendo todos los usuarios");
        var response = await _userService.GetAllUsersAsync();
        return Ok(response);
    }

    /// <summary>
    /// Obtiene usuarios activos
    /// </summary>
    /// <returns>Lista de usuarios activos</returns>
    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetActiveUsers()
    {
        _logger.LogInformation("Obteniendo usuarios activos");
        var response = await _userService.GetActiveUsersAsync();
        return Ok(response);
    }

    /// <summary>
    /// Obtiene un usuario por su ID
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <returns>Usuario encontrado</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetUserById(Guid id)
    {
        _logger.LogInformation("Obteniendo usuario con ID: {UserId}", id);
        var response = await _userService.GetUserByIdAsync(id);
        
        if (!response.Success)
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    /// <summary>
    /// Obtiene un usuario por su email
    /// </summary>
    /// <param name="email">Email del usuario</param>
    /// <returns>Usuario encontrado</returns>
    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetUserByEmail(string email)
    {
        _logger.LogInformation("Obteniendo usuario con email: {Email}", email);
        var response = await _userService.GetUserByEmailAsync(email);
        
        if (!response.Success)
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    /// <summary>
    /// Crea un nuevo usuario
    /// </summary>
    /// <param name="createUserDto">Datos del nuevo usuario</param>
    /// <returns>Usuario creado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<UserDto>>> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        _logger.LogInformation("Creando nuevo usuario con email: {Email}", createUserDto.Email);
        var response = await _userService.CreateUserAsync(createUserDto);
        
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return CreatedAtAction(nameof(GetUserById), new { id = response.Data!.Id }, response);
    }

    /// <summary>
    /// Actualiza un usuario existente
    /// </summary>
    /// <param name="id">ID del usuario a actualizar</param>
    /// <param name="updateUserDto">Datos actualizados</param>
    /// <returns>Usuario actualizado</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<UserDto>>> UpdateUser(Guid id, [FromBody] UpdateUserDto updateUserDto)
    {
        _logger.LogInformation("Actualizando usuario con ID: {UserId}", id);
        var response = await _userService.UpdateUserAsync(id, updateUserDto);
        
        if (!response.Success)
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    /// <summary>
    /// Elimina un usuario
    /// </summary>
    /// <param name="id">ID del usuario a eliminar</param>
    /// <returns>Resultado de la operación</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteUser(Guid id)
    {
        _logger.LogInformation("Eliminando usuario con ID: {UserId}", id);
        var response = await _userService.DeleteUserAsync(id);
        
        if (!response.Success)
        {
            return NotFound(response);
        }

        return Ok(response);
    }
}
