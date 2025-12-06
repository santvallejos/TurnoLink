using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TurnoLink.Business.DTOs;
using TurnoLink.Business.Interfaces;

namespace TurnoLink.WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing bookings by professionals (requires authentication)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize] // Requires authentication
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(IBookingService bookingService, ILogger<BookingsController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        /// <summary>
        /// Get all bookings for the authenticated professional
        /// </summary>
        [HttpGet("my-bookings")]
        [ProducesResponseType(typeof(IEnumerable<BookingDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetMyBookings()
        {
            var userId = GetAuthenticatedUserId();
            _logger.LogInformation("Getting bookings for professional: {UserId}", userId);
            
            var bookings = await _bookingService.GetBookingsByUserIdAsync(userId);
            return Ok(bookings);
        }

        /// <summary>
        /// Get bookings by date range
        /// </summary>
        [HttpGet("my-bookings/range")]
        [ProducesResponseType(typeof(IEnumerable<BookingDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingsByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var userId = GetAuthenticatedUserId();
            _logger.LogInformation("Getting bookings for professional {UserId} between {StartDate} and {EndDate}", 
                userId, startDate, endDate);
            
            var allBookings = await _bookingService.GetBookingsByUserIdAsync(userId);
            var filteredBookings = allBookings.Where(b => 
                b.StartTime >= startDate && b.StartTime <= endDate);
            
            return Ok(filteredBookings);
        }

        /// <summary>
        /// Gets details of a specific booking by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingDto>> GetBookingById(Guid id)
        {
            _logger.LogInformation("Getting booking: {BookingId}", id);
            
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
                return NotFound(new { message = "Booking not found" });

            // Verify that the booking belongs to the authenticated professional
            var userId = GetAuthenticatedUserId();
            if (booking.UserId != userId)
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "You do not have permission to view this booking" });

            return Ok(booking);
        }

        /// <summary>
        /// Updates the status of a booking
        /// </summary>
        [HttpPatch("{id:guid}")]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<BookingDto>> UpdateBooking(Guid id, [FromBody] UpdateBookingDto updateBookingDto)
        {
            try
            {
                // First verify that the booking exists and belongs to the professional
                var existingBooking = await _bookingService.GetBookingByIdAsync(id);
                if (existingBooking == null)
                    return NotFound(new { message = "Booking not found" });

                var userId = GetAuthenticatedUserId();
                if (existingBooking.UserId != userId)
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = "You do not have permission to modify this booking" });

                _logger.LogInformation("Updating booking: {BookingId} by professional: {UserId}", id, userId);
                
                var booking = await _bookingService.UpdateBookingAsync(id, updateBookingDto);
                return Ok(booking);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Cancels a booking
        /// </summary>
        [HttpPost("{id:guid}/cancel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> CancelBooking(Guid id)
        {
            try
            {
                // Verify that the booking exists and belongs to the professional
                var existingBooking = await _bookingService.GetBookingByIdAsync(id);
                if (existingBooking == null)
                    return NotFound(new { message = "Booking not found" });

                var userId = GetAuthenticatedUserId();
                if (existingBooking.UserId != userId)
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = "You do not have permission to cancel this booking" });

                _logger.LogInformation("Canceling booking: {BookingId} by professional: {UserId}", id, userId);
                
                await _bookingService.CancelBookingAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
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
