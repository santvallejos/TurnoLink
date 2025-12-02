using TurnoLink.DataAccess.Entities;

namespace TurnoLink.DataAccess.Repositories.Interfaces;

/// <summary>
/// Interfaz específica para repositorio de notificaciones
/// </summary>
public interface INotificationRepository : IRepository<Notification>
{
    /// <summary>
    /// Obtiene notificaciones pendientes de envío
    /// </summary>
    Task<IEnumerable<Notification>> GetPendingNotificationsAsync();

    /// <summary>
    /// Obtiene notificaciones de una reserva
    /// </summary>
    Task<IEnumerable<Notification>> GetNotificationsByBookingIdAsync(Guid bookingId);
}
