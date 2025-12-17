using Microsoft.AspNetCore.SignalR;
using TurnoLink.Business.DTOs;
using TurnoLink.Business.Hubs;
using TurnoLink.Business.Interfaces;

namespace TurnoLink.Business.Services
{
    /// <summary>
    /// Servicio para enviar notificaciones en tiempo real via SignalR
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        /// <summary>
        /// Constructor del servicio de notificaciones
        /// </summary>
        /// <param name="hubContext">Contexto del hub de SignalR</param>
        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        /// <summary>
        /// Notifica a un profesional sobre una nueva reserva
        /// </summary>
        /// <param name="userId">ID del profesional</param>
        /// <param name="booking">Datos de la reserva</param>
        public async Task NotifyNewBookingAsync(Guid userId, BookingDto booking)
        {
            var notification = new BookingNotificationDto
            {
                Type = "NewBooking",
                BookingId = booking.Id,
                ClientName = booking.ClientName,
                ServiceName = booking.ServiceName,
                StartTime = booking.StartTime,
                CreatedAt = DateTime.UtcNow,
                Message = $"Nueva reserva de {booking.ClientName} para {booking.ServiceName}"
            };

            // Enviar notificación al grupo del usuario específico
            await _hubContext.Clients.Group($"user_{userId}").SendAsync("ReceiveBookingNotification", notification);
        }
    }
}
