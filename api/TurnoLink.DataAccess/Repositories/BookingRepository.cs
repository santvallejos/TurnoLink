using Microsoft.EntityFrameworkCore;
using TurnoLink.DataAccess.Data;
using TurnoLink.DataAccess.Entities;
using TurnoLink.DataAccess.Repositories.Interfaces;

namespace TurnoLink.DataAccess.Repositories;

/// <summary>
/// Implementación del repositorio de reservas
/// </summary>
public class BookingRepository : Repository<Booking>, IBookingRepository
{
    public BookingRepository(TurnoLinkDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Booking>> GetBookingsByClientIdAsync(Guid clientId)
    {
        return await _dbSet
            .Include(b => b.Service)
            .Include(b => b.User)
            .Where(b => b.ClientId == clientId)
            .OrderByDescending(b => b.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Include(b => b.Client)
            .Include(b => b.Service)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(b => b.Client)
            .Include(b => b.Service)
            .Include(b => b.User)
            .Where(b => b.StartTime >= startDate && b.StartTime <= endDate)
            .OrderBy(b => b.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetBookingsByUserAndDateAsync(Guid userId, DateTime date)
    {
        var startOfDay = date.Date;
        var endOfDay = startOfDay.AddDays(1);

        return await _dbSet
            .Include(b => b.Client)
            .Include(b => b.Service)
            .Where(b => b.UserId == userId 
                && b.StartTime >= startOfDay 
                && b.StartTime < endOfDay)
            .OrderBy(b => b.StartTime)
            .ToListAsync();
    }

    public async Task<bool> HasTimeConflictAsync(Guid userId, DateTime startTime, DateTime endTime, Guid? excludeBookingId = null)
    {
        var query = _dbSet.Where(b => 
            b.UserId == userId
            && b.Status != BookingStatus.Cancelled
            && b.StartTime < endTime
            && b.EndTime > startTime);

        if (excludeBookingId.HasValue)
        {
            query = query.Where(b => b.Id != excludeBookingId.Value);
        }

        return await query.AnyAsync();
    }

    public override async Task<Booking?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(b => b.Client)
            .Include(b => b.Service)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == id);
    }
}
