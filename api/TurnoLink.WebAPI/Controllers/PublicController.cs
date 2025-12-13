using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurnoLink.Business.DTOs;
using TurnoLink.Business.Interfaces;

namespace TurnoLink.WebAPI.Controllers
{
    /// <summary>
    /// Public controller for clients (no authentication required)
    /// Allows viewing services and creating bookings
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [AllowAnonymous] // Public - no authentication required
    public class PublicController : ControllerBase
    {
        private readonly IServiceService _serviceService;
        private readonly IBookingService _bookingService;
        private readonly ILogger<PublicController> _logger;

        public PublicController(
            IServiceService serviceService,
            IBookingService bookingService,
            ILogger<PublicController> logger)
        {
            _serviceService = serviceService;
            _bookingService = bookingService;
            _logger = logger;
        }

        /// <summary>
        /// Gets details of a specific service
        /// </summary>
        [HttpGet("services/{id:guid}")]
        [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceDto>> GetServiceById(Guid id)
        {
            _logger.LogInformation("Client requesting service: {ServiceId}", id);
            
            var service = await _serviceService.GetServiceByIdAsync(id);
            if (service == null)
                return NotFound(new { message = "Service not found" });

            return Ok(service);
        }

        /// <summary>
        /// Gets all services for User ID
        /// </summary>
        [HttpGet("{slug}")]
        [ProducesResponseType(typeof(IEnumerable<ServiceDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetServicesBySlugUser(string slug)
        {
            var services = await _serviceService.GetServicesBySlugAsync(slug);
            if (services == null)
                return NotFound(new { message = $"{slug}" });

            return Ok(services);
        }

        /// <summary>
        /// Creates a new booking (client provides their details)
        /// </summary>
        [HttpPost("bookings")]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BookingDto>> CreateBooking([FromBody] CreateBookingDto createBookingDto)
        {
            try
            {
                _logger.LogInformation("Client creating booking for service: {ServiceId}", createBookingDto.ServiceId);
                
                var booking = await _bookingService.CreateBookingAsync(createBookingDto);
                return booking;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Error creating booking: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Gets details of a booking by ID
        /// </summary>
        [HttpGet("bookings/{id:guid}")]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingDto>> GetBookingById(Guid id)
        {
            _logger.LogInformation("Client requesting booking: {BookingId}", id);
            
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
                return NotFound(new { message = "Booking not found" });

            return Ok(booking);
        }
    }

    /// <summary>
    /// DTO for checking availability
    /// </summary>
    public class CheckAvailabilityDto
    {
        public Guid UserId { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set; }
    }
}
