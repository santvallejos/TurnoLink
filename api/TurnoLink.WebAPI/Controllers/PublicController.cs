using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurnoLink.Business.DTOs;
using TurnoLink.Business.Interfaces;

namespace TurnoLink.WebAPI.Controllers
{
    /// <summary>
    /// Controlador público para clientes (sin autenticación requerida)
    /// Permite ver servicios y crear reservas
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [AllowAnonymous] // Público - no requiere autenticación
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
        /// Obtiene todos los servicios activos disponibles
        /// </summary>
        [HttpGet("services")]
        [ProducesResponseType(typeof(IEnumerable<ServiceDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetAllActiveServices()
        {
            _logger.LogInformation("Cliente consultando servicios disponibles");
            var services = await _serviceService.GetAllActiveServicesAsync();
            return Ok(services);
        }

        /// <summary>
        /// Obtiene servicios activos de un profesional específico
        /// </summary>
        [HttpGet("services/professional/{userId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<ServiceDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetServicesByProfessional(Guid userId)
        {
            _logger.LogInformation("Cliente consultando servicios del profesional: {UserId}", userId);
            var services = await _serviceService.GetActiveServicesByUserIdAsync(userId);
            return Ok(services);
        }

        /// <summary>
        /// Obtiene detalles de un servicio específico
        /// </summary>
        [HttpGet("services/{id:guid}")]
        [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceDto>> GetServiceById(Guid id)
        {
            _logger.LogInformation("Cliente consultando servicio: {ServiceId}", id);
            
            var service = await _serviceService.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound(new { message = "Servicio no encontrado" });
            }

            return Ok(service);
        }

        /// <summary>
        /// Crea una nueva reserva (cliente proporciona sus datos)
        /// </summary>
        [HttpPost("bookings")]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BookingDto>> CreateBooking([FromBody] CreateBookingDto createBookingDto)
        {
            try
            {
                _logger.LogInformation("Cliente creando reserva para servicio: {ServiceId}", createBookingDto.ServiceId);
                
                var booking = await _bookingService.CreateBookingAsync(createBookingDto);
                return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Error al crear reserva: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene detalles de una reserva por ID
        /// </summary>
        [HttpGet("bookings/{id:guid}")]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingDto>> GetBookingById(Guid id)
        {
            _logger.LogInformation("Consultando reserva: {BookingId}", id);
            
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound(new { message = "Reserva no encontrada" });
            }

            return Ok(booking);
        }

        /// <summary>
        /// Verifica disponibilidad para un horario específico
        /// </summary>
        [HttpPost("bookings/check-availability")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<ActionResult<object>> CheckAvailability([FromBody] CheckAvailabilityDto checkAvailabilityDto)
        {
            _logger.LogInformation("Verificando disponibilidad");
            
            var isAvailable = await _bookingService.CheckAvailabilityAsync(
                checkAvailabilityDto.UserId,
                checkAvailabilityDto.StartTime,
                checkAvailabilityDto.DurationMinutes);

            return Ok(new { available = isAvailable });
        }
    }

    /// <summary>
    /// DTO para verificar disponibilidad
    /// </summary>
    public class CheckAvailabilityDto
    {
        public Guid UserId { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set; }
    }
}
