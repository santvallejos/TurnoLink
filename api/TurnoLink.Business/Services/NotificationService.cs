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

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

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

            await _hubContext.Clients.Group($"user_{userId}").SendAsync("ReceiveBookingNotification", notification); // Send notifications to the specific user group
        }
    }
}
