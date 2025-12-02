using TurnoLink.Business.DTOs;

namespace TurnoLink.Business.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de gestión de reservas
    /// </summary>
    public interface IBookingService
    {
        Task<BookingDto> CreateBookingAsync(CreateBookingDto createBookingDto);
        Task<BookingDto?> GetBookingByIdAsync(Guid id);
        Task<IEnumerable<BookingDto>> GetBookingsByClientIdAsync(Guid clientId);
        Task<IEnumerable<BookingDto>> GetBookingsByUserIdAsync(Guid userId);
        Task<IEnumerable<BookingDto>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<BookingDto> UpdateBookingAsync(Guid id, UpdateBookingDto updateBookingDto);
        Task<bool> CancelBookingAsync(Guid id);
        Task<bool> CheckAvailabilityAsync(Guid userId, DateTime startTime, int durationMinutes);
    }
}
