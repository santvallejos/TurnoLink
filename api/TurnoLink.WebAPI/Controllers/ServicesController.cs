using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TurnoLink.Business.DTOs;
using TurnoLink.Business.Interfaces;

namespace TurnoLink.WebAPI.Controllers
{
    /// <summary>
    /// Controlador para gestión de servicios por profesionales (requiere autenticación)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize] // Requiere autenticación para todos los endpoints
    public class ServicesController : ControllerBase
    {
        private readonly IServiceService _serviceService;
        private readonly ILogger<ServicesController> _logger;

        public ServicesController(IServiceService serviceService, ILogger<ServicesController> logger)
        {
            _serviceService = serviceService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los servicios del profesional autenticado
        /// </summary>
        [HttpGet("my-services")]
        [ProducesResponseType(typeof(IEnumerable<ServiceDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetMyServices()
        {
            var userId = GetAuthenticatedUserId();
            _logger.LogInformation("Obteniendo servicios del usuario: {UserId}", userId);
            
            var services = await _serviceService.GetServicesByUserIdAsync(userId);
            return Ok(services);
        }

        /// <summary>
        /// Obtiene servicios activos del profesional autenticado
        /// </summary>
        [HttpGet("my-services/active")]
        [ProducesResponseType(typeof(IEnumerable<ServiceDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetMyActiveServices()
        {
            var userId = GetAuthenticatedUserId();
            _logger.LogInformation("Obteniendo servicios activos del usuario: {UserId}", userId);
            
            var services = await _serviceService.GetActiveServicesByUserIdAsync(userId);
            return Ok(services);
        }

        /// <summary>
        /// Obtiene un servicio específico por ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceDto>> GetServiceById(Guid id)
        {
            _logger.LogInformation("Obteniendo servicio: {ServiceId}", id);
            
            var service = await _serviceService.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound(new { message = "Servicio no encontrado" });
            }

            return Ok(service);
        }

        /// <summary>
        /// Crea un nuevo servicio
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServiceDto>> CreateService([FromBody] CreateServiceDto createServiceDto)
        {
            try
            {
                var userId = GetAuthenticatedUserId();
                _logger.LogInformation("Creando servicio para usuario: {UserId}", userId);
                
                var service = await _serviceService.CreateServiceAsync(userId, createServiceDto);
                return CreatedAtAction(nameof(GetServiceById), new { id = service.Id }, service);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un servicio existente
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ServiceDto>> UpdateService(Guid id, [FromBody] UpdateServiceDto updateServiceDto)
        {
            try
            {
                var userId = GetAuthenticatedUserId();
                _logger.LogInformation("Actualizando servicio: {ServiceId} por usuario: {UserId}", id, userId);
                
                var service = await _serviceService.UpdateServiceAsync(userId, id, updateServiceDto);
                return Ok(service);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un servicio
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> DeleteService(Guid id)
        {
            try
            {
                var userId = GetAuthenticatedUserId();
                _logger.LogInformation("Eliminando servicio: {ServiceId} por usuario: {UserId}", id, userId);
                
                await _serviceService.DeleteServiceAsync(userId, id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
        }

        private Guid GetAuthenticatedUserId()
        {
            var userIdClaim = User.FindFirst("userId")?.Value 
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("Usuario no autenticado");
            }

            return userId;
        }
    }
}
