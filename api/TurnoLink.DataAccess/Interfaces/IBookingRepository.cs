using TurnoLink.DataAccess.Entities;

namespace TurnoLink.DataAccess.Interfaces
{
    /// <summary>
    /// Interface of the booking repository
    /// </summary>
    public interface IBookingRepository
    {
        /// <summary>
        /// Gets all bookings
        /// </summary>
        Task<IEnumerable<Booking>> GetAllAsync();

        /// <summary>
        /// Gets a booking by its ID
        /// </summary>
        /// <param name="id">Booking ID</param>
        Task<Booking?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets bookings by client ID
        /// </summary>
        /// <param name="clientId">Client ID</param>
        Task<IEnumerable<Booking>> GetBookingsByClientIdAsync(Guid clientId);

        /// <summary>
        /// Gets bookings by user ID
        /// </summary>
        /// <param name="userId">User ID</param>
        Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(Guid userId);

        /// <summary>
        /// Gets bookings by date range
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets bookings by user and date
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="date">Date</param>
        Task<IEnumerable<Booking>> GetBookingsByUserAndDateAsync(Guid userId, DateTime date);

        /// <summary>
        /// Adds a new booking
        /// </summary>
        /// <param name="entity">Booking entity</param>
        Task<Booking> AddAsync(Booking entity);

        /// <summary>
        /// Updates an existing booking
        /// </summary>
        /// <param name="entity">Booking entity</param>
        void Update(Booking entity);

        /// <summary>
        /// Deletes a booking
        /// </summary>
        /// <param name="entity">Booking entity</param>
        void Remove(Booking entity);

        /// <summary>
        /// Checks if a booking exists
        /// </summary>
        /// <param name="predicate">Condition to check</param>
        Task<bool> ExistsAsync(Func<Booking, bool> predicate);
    }
}