using TurnoLink.Business.DTOs;

namespace TurnoLink.Business.Services.Interfaces;

/// <summary>
/// Interfaz para el servicio de gestión de reservas
/// </summary>
public interface IBookingService
{
    Task<ApiResponse<BookingDto>> CreateBookingAsync(CreateBookingDto createBookingDto);
    Task<ApiResponse<BookingDto>> GetBookingByIdAsync(Guid id);
    Task<ApiResponse<IEnumerable<BookingDto>>> GetBookingsByClientIdAsync(Guid clientId);
    Task<ApiResponse<IEnumerable<BookingDto>>> GetBookingsByUserIdAsync(Guid userId);
    Task<ApiResponse<IEnumerable<BookingDto>>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<ApiResponse<BookingDto>> UpdateBookingAsync(Guid id, UpdateBookingDto updateBookingDto);
    Task<ApiResponse<bool>> CancelBookingAsync(Guid id);
    Task<ApiResponse<bool>> CheckAvailabilityAsync(Guid userId, DateTime startTime, int durationMinutes);
}
