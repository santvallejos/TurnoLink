using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TurnoLink.Business.DTOs;
using TurnoLink.Business.Interfaces;

namespace TurnoLink.WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing services by professionals (requires authentication)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize] // Requires authentication for all endpoints
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
        /// Gets all services of the authenticated professional
        /// </summary>
        [HttpGet("my-services")]
        [ProducesResponseType(typeof(IEnumerable<ServiceDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetMyServices()
        {
            var userId = GetAuthenticatedUserId();
            _logger.LogInformation("Getting services for user: {UserId}", userId);
            
            var services = await _serviceService.GetServicesByUserIdAsync(userId);
            return Ok(services);
        }

        /// <summary>
        /// Gets active services of the authenticated professional
        /// </summary>
        [HttpGet("my-services/active")]
        [ProducesResponseType(typeof(IEnumerable<ServiceDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetMyActiveServices()
        {
            var userId = GetAuthenticatedUserId();
            _logger.LogInformation("Getting active services for user: {UserId}", userId);
            
            var services = await _serviceService.GetActiveServicesByUserIdAsync(userId);
            return Ok(services);
        }

        /// <summary>
        /// Gets a specific service by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceDto>> GetServiceById(Guid id)
        {
            _logger.LogInformation("Getting service with ID: {ServiceId}", id);
            
            var service = await _serviceService.GetServiceByIdAsync(id);
            if (service == null)
                return NotFound(new { message = "Service not found" });

            return Ok(service);
        }

        /// <summary>
        /// Creates a new service
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServiceDto>> CreateService([FromBody] CreateServiceDto createServiceDto)
        {
            try
            {
                var userId = GetAuthenticatedUserId();
                _logger.LogInformation("Creating service for user: {UserId}", userId);
                
                var service = await _serviceService.CreateServiceAsync(userId, createServiceDto);
                return CreatedAtAction(nameof(GetServiceById), new { id = service.Id }, service);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing service
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
                _logger.LogInformation("Updating service: {ServiceId} by user: {UserId}", id, userId);
                
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
        /// Deletes a service
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
                _logger.LogInformation("Deleting service: {ServiceId} by user: {UserId}", id, userId);
                
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
                throw new UnauthorizedAccessException("User not authenticated");

            return userId;
        }
    }
}
