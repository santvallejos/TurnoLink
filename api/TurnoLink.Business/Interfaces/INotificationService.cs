using TurnoLink.Business.DTOs;

namespace TurnoLink.Business.Interfaces
{
    /// <summary>
    /// Interface para el servicio de notificaciones en tiempo real
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Notifica a un profesional sobre una nueva reserva
        /// </summary>
        /// <param name="userId">ID del profesional</param>
        /// <param name="booking">Datos de la reserva</param>
        Task NotifyNewBookingAsync(Guid userId, BookingDto booking);
    }
}
