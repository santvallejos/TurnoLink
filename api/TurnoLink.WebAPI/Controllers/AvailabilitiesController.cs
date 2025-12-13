using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TurnoLink.Business.DTOs;
using TurnoLink.Business.Interfaces;

namespace TurnoLink.WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing availability slots
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AvailabilitiesController : ControllerBase
    {
        private readonly IAvailabilityService _availabilityService;

        public AvailabilitiesController(IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        /// <summary>
        /// Gets an availability by ID
        /// </summary>
        /// <param name="id">Availability ID</param>
        /// <returns>Availability details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AvailabilityDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AvailabilityDto>> GetById(Guid id)
        {
            var availability = await _availabilityService.GetByIdAsync(id);
            if (availability == null)
                return NotFound(new { message = "Availability not found" });

            return Ok(availability);
        }

        /// <summary>
        /// Gets all availabilities for the authenticated user
        /// </summary>
        /// <returns>List of availabilities</returns>
        [HttpGet("my-availabilities")]
        [ProducesResponseType(typeof(IEnumerable<AvailabilityDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AvailabilityDto>>> GetMyAvailabilities()
        {
            var userId = GetUserIdFromClaims();
            var availabilities = await _availabilityService.GetAvailabilitiesByUserIdAsync(userId);
            return Ok(availabilities);
        }

        /// <summary>
        /// Gets availabilities by user ID (public endpoint for clients)
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of availabilities</returns>
        [HttpGet("user/{userId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<AvailabilityDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AvailabilityDto>>> GetByUserId(Guid userId)
        {
            var availabilities = await _availabilityService.GetAvailabilitiesByUserIdAsync(userId);
            return Ok(availabilities);
        }

        /// <summary>
        /// Gets availabilities by service ID
        /// </summary>
        /// <param name="serviceId">Service ID</param>
        /// <returns>List of availabilities</returns>
        [HttpGet("service/{serviceId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<AvailabilityDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AvailabilityDto>>> GetByServiceId(Guid serviceId)
        {
            var availabilities = await _availabilityService.GetAvailabilitiesByServiceIdAsync(serviceId);
            return Ok(availabilities);
        }

        /// <summary>
        /// Gets available slots for a user within a date range
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="startDate">Start date (ISO 8601 format)</param>
        /// <param name="endDate">End date (ISO 8601 format)</param>
        /// <returns>List of available slots</returns>
        [HttpGet("user/{userId}/range")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<AvailabilityDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<AvailabilityDto>>> GetByDateRange(
            Guid userId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            if (endDate < startDate)
                return BadRequest(new { message = "End date must be after start date" });

            var availabilities = await _availabilityService.GetAvailableSlotsByDateRangeAsync(userId, startDate, endDate);
            return Ok(availabilities);
        }

        /// <summary>
        /// Creates a new availability slot
        /// </summary>
        /// <param name="createDto">Availability creation data</param>
        /// <returns>Created availability</returns>
        [HttpPost]
        [ProducesResponseType(typeof(AvailabilityDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AvailabilityDto>> Create([FromBody] CreateAvailabilityDto createDto)
        {
            try
            {
                var userId = GetUserIdFromClaims();
                var availability = await _availabilityService.CreateAvailabilityAsync(userId, createDto);
                return CreatedAtAction(nameof(GetById), new { id = availability }, availability);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing availability
        /// </summary>
        /// <param name="id">Availability ID</param>
        /// <param name="updateDto">Update data</param>
        /// <returns>Updated availability</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AvailabilityDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AvailabilityDto>> Update(Guid id, [FromBody] UpdateAvailabilityDto updateDto)
        {
            try
            {
                var userId = GetUserIdFromClaims();
                var availability = await _availabilityService.UpdateAvailabilityAsync(id, userId, updateDto);
                return Ok(availability);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Deletes an availability slot
        /// </summary>
        /// <param name="id">Availability ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var userId = GetUserIdFromClaims();
                await _availabilityService.DeleteAvailabilityAsync(id, userId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Gets the user ID from JWT claims
        /// </summary>
        private Guid GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException("Usuario no autenticado");

            return userId;
        }
    }
}
