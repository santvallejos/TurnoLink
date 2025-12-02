using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TurnoLink.Business.DTOs;
using TurnoLink.Business.Interfaces;

namespace TurnoLink.WebAPI.Controllers
{
    /// <summary>
    /// Controlador para gestión de reservas por profesionales (requiere autenticación)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize] // Requiere autenticación
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
        /// Obtiene todas las reservas del profesional autenticado
        /// </summary>
        [HttpGet("my-bookings")]
        [ProducesResponseType(typeof(IEnumerable<BookingDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetMyBookings()
        {
            var userId = GetAuthenticatedUserId();
            _logger.LogInformation("Obteniendo reservas del profesional: {UserId}", userId);
            
            var bookings = await _bookingService.GetBookingsByUserIdAsync(userId);
            return Ok(bookings);
        }

        /// <summary>
        /// Obtiene reservas por rango de fechas
        /// </summary>
        [HttpGet("my-bookings/range")]
        [ProducesResponseType(typeof(IEnumerable<BookingDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingsByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var userId = GetAuthenticatedUserId();
            _logger.LogInformation("Obteniendo reservas del profesional {UserId} entre {StartDate} y {EndDate}", 
                userId, startDate, endDate);
            
            var allBookings = await _bookingService.GetBookingsByUserIdAsync(userId);
            var filteredBookings = allBookings.Where(b => 
                b.StartTime >= startDate && b.StartTime <= endDate);
            
            return Ok(filteredBookings);
        }

        /// <summary>
        /// Obtiene detalles de una reserva específica
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingDto>> GetBookingById(Guid id)
        {
            _logger.LogInformation("Obteniendo reserva: {BookingId}", id);
            
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound(new { message = "Reserva no encontrada" });
            }

            // Verificar que la reserva pertenece al profesional autenticado
            var userId = GetAuthenticatedUserId();
            if (booking.UserId != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, 
                    new { message = "No tienes permiso para ver esta reserva" });
            }

            return Ok(booking);
        }

        /// <summary>
        /// Actualiza el estado de una reserva
        /// </summary>
        [HttpPatch("{id:guid}")]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<BookingDto>> UpdateBooking(Guid id, [FromBody] UpdateBookingDto updateBookingDto)
        {
            try
            {
                // Primero verificar que la reserva existe y pertenece al profesional
                var existingBooking = await _bookingService.GetBookingByIdAsync(id);
                if (existingBooking == null)
                {
                    return NotFound(new { message = "Reserva no encontrada" });
                }

                var userId = GetAuthenticatedUserId();
                if (existingBooking.UserId != userId)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, 
                        new { message = "No tienes permiso para modificar esta reserva" });
                }

                _logger.LogInformation("Actualizando reserva: {BookingId} por profesional: {UserId}", id, userId);
                
                var booking = await _bookingService.UpdateBookingAsync(id, updateBookingDto);
                return Ok(booking);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Cancela una reserva
        /// </summary>
        [HttpPost("{id:guid}/cancel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> CancelBooking(Guid id)
        {
            try
            {
                // Verificar que la reserva existe y pertenece al profesional
                var existingBooking = await _bookingService.GetBookingByIdAsync(id);
                if (existingBooking == null)
                {
                    return NotFound(new { message = "Reserva no encontrada" });
                }

                var userId = GetAuthenticatedUserId();
                if (existingBooking.UserId != userId)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, 
                        new { message = "No tienes permiso para cancelar esta reserva" });
                }

                _logger.LogInformation("Cancelando reserva: {BookingId} por profesional: {UserId}", id, userId);
                
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
            {
                throw new UnauthorizedAccessException("Usuario no autenticado");
            }

            return userId;
        }
    }
}
