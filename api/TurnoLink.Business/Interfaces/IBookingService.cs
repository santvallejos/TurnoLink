using TurnoLink.Business.DTOs;

namespace TurnoLink.Business.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de gestión de reservas
    /// </summary>
    public interface IBookingService
    {
        /// <summary>
        /// Crea una nueva reserva (público - para clientes)
        /// </summary>
        Task<BookingDto> CreateBookingAsync(CreateBookingDto createBookingDto);

        /// <summary>
        /// Obtiene una reserva por su ID
        /// </summary>
        Task<BookingDto?> GetBookingByIdAsync(Guid id);

        /// <summary>
        /// Obtiene reservas por ID de cliente
        /// </summary>
        Task<IEnumerable<BookingDto>> GetBookingsByClientIdAsync(Guid clientId);

        /// <summary>
        /// Obtiene reservas por ID de usuario/profesional
        /// </summary>
        Task<IEnumerable<BookingDto>> GetBookingsByUserIdAsync(Guid userId);

        /// <summary>
        /// Obtiene reservas por rango de fechas
        /// </summary>
        Task<IEnumerable<BookingDto>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Actualiza una reserva existente
        /// </summary>
        Task<BookingDto> UpdateBookingAsync(Guid id, UpdateBookingDto updateBookingDto);

        /// <summary>
        /// Cancela una reserva
        /// </summary>
        Task<bool> CancelBookingAsync(Guid id);

        /// <summary>
        /// Verifica disponibilidad para un servicio en fecha/hora específica
        /// </summary>
        Task<bool> CheckAvailabilityAsync(Guid userId, DateTime startTime, int durationMinutes);
    }
}
