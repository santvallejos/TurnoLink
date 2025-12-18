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
        /// Creates a new availability slot.
        /// The client sends local date/time with UTC offset, and the backend converts to UTC.
        /// End time is automatically calculated based on service duration.
        /// </summary>
        /// <remarks>
        /// Example request:
        /// {
        ///     "serviceId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///     "startDate": "2025-12-24",
        ///     "startTime": "15:00",
        ///     "utcOffsetMinutes": -180  // UTC-3 (Argentina)
        /// }
        /// 
        /// The utcOffsetMinutes is the client's timezone offset in minutes:
        /// - Negative values for west of UTC (e.g., -180 for UTC-3)
        /// - Positive values for east of UTC (e.g., 60 for UTC+1)
        /// </remarks>
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
                return CreatedAtAction(nameof(GetById), new { id = availability.Id }, availability);
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
        /// Creates recurring availability slots from start date to end date.
        /// All slots will have the same time (converted to UTC) based on repeat frequency.
        /// </summary>
        /// <remarks>
        /// Example request:
        /// {
        ///     "serviceId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///     "startDate": "2025-12-24",
        ///     "startTime": "15:00",
        ///     "utcOffsetMinutes": -180,
        ///     "repeat": 2,  // 1=Daily, 2=Weekly, 3=Monthly
        ///     "endDate": "2026-01-24"
        /// }
        /// 
        /// Repeat values:
        /// - 1: Daily - creates a slot every day
        /// - 2: Weekly - creates a slot every week on the same day
        /// - 3: Monthly - creates a slot every month on the same date
        /// </remarks>
        [HttpPost("recurring")]
        [ProducesResponseType(typeof(IEnumerable<AvailabilityDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<AvailabilityDto>>> CreateRecurring([FromBody] CreateRecurringAvailabilityDto createDto)
        {
            try 
            {
                var userId = GetUserIdFromClaims();
                var availabilities = await _availabilityService.CreateRecurringAvailabilityAsync(userId, createDto);
                return CreatedAtAction(nameof(GetMyAvailabilities), availabilities);
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
        /// Updates an existing availability's date and/or time
        /// </summary>
        /// <remarks>
        /// Example request:
        /// {
        ///     "newDate": "2025-12-26",  // Optional: new date
        ///     "newTime": "16:00",  // Optional: new time
        ///     "utcOffsetMinutes": -180  // Required when updating date or time
        /// }
        /// 
        /// Note: UTC offset is required when changing date or time to properly convert to UTC.
        /// </remarks>
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
