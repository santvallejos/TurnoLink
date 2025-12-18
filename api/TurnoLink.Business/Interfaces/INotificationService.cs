using TurnoLink.Business.DTOs;

namespace TurnoLink.Business.Interfaces
{
    /// <summary>
    /// Interface for notifications service
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Notifies a professional about a new booking
        /// </summary>
        /// <param name="userId">ID of the professional</param>
        /// <param name="booking">Booking data</param>
        Task NotifyNewBookingAsync(Guid userId, BookingDto booking);
    }
}
