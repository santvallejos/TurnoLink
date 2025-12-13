using TurnoLink.Business.DTOs;

namespace TurnoLink.Business.Interfaces
{
    /// <summary>
    /// Interface for booking-related operations.
    /// </summary>
    public interface IBookingService
    {
        /// <summary>
        /// Gets a booking by its ID
        /// </summary>
        /// <param name="id">Booking ID</param>
        Task<BookingDto?> GetBookingByIdAsync(Guid id);

        /// <summary>
        /// Gets bookings by client ID
        /// </summary>
        /// <param name="clientId">Client's user ID</param>
        Task<IEnumerable<BookingDto>> GetBookingsByClientIdAsync(Guid clientId);

        /// <summary>
        /// Gets all bookings by user/professional ID
        /// </summary>
        /// <param name="userId">User/Professional's user ID</param>
        Task<IEnumerable<BookingDto>> GetBookingsByUserIdAsync(Guid userId);

        /// <summary>
        /// Gets bookings within a date range
        /// </summary>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        Task<IEnumerable<BookingDto>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Creates a new booking (public - for clients)
        /// </summary>
        /// <param name="createBookingDto">DTO containing booking creation data</param>
        Task<BookingDto> CreateBookingAsync(CreateBookingDto createBookingDto);

        /// <summary>
        /// Updates an existing booking
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <param name="updateBookingDto">DTO containing booking update data</param>
        Task<BookingDto> UpdateBookingAsync(Guid id, UpdateBookingDto updateBookingDto);

        /// <summary>
        /// Cancels a booking
        /// </summary>
        /// <param name="id">Booking ID</param>
        Task<bool> CancelBookingAsync(Guid id);
    }
}
