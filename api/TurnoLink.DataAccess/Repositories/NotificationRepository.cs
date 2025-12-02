using Microsoft.EntityFrameworkCore;
using TurnoLink.DataAccess.Data;
using TurnoLink.DataAccess.Entities;
using TurnoLink.DataAccess.Repositories.Interfaces;

namespace TurnoLink.DataAccess.Repositories;

/// <summary>
/// Implementación del repositorio de notificaciones
/// </summary>
public class NotificationRepository : Repository<Notification>, INotificationRepository
{
    public NotificationRepository(TurnoLinkDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Notification>> GetPendingNotificationsAsync()
    {
        return await _dbSet
            .Where(n => n.Status == NotificationStatus.Pending)
            .OrderBy(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Notification>> GetNotificationsByBookingIdAsync(Guid bookingId)
    {
        return await _dbSet
            .Where(n => n.BookingId == bookingId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }
}
