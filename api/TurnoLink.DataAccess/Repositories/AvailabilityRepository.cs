using Microsoft.EntityFrameworkCore;
using TurnoLink.DataAccess.Data;
using TurnoLink.DataAccess.Entities;
using TurnoLink.DataAccess.Repositories.Interfaces;

namespace TurnoLink.DataAccess.Repositories;

/// <summary>
/// Implementación del repositorio de disponibilidades
/// </summary>
public class AvailabilityRepository : Repository<Availability>, IAvailabilityRepository
{
    public AvailabilityRepository(TurnoLinkDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Availability>> GetActiveAvailabilitiesByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Where(a => a.UserId == userId && a.IsActive)
            .OrderBy(a => a.DayOfWeek)
            .ThenBy(a => a.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Availability>> GetAvailabilitiesByUserAndDayAsync(Guid userId, int dayOfWeek)
    {
        return await _dbSet
            .Where(a => a.UserId == userId && a.DayOfWeek == dayOfWeek && a.IsActive)
            .OrderBy(a => a.StartTime)
            .ToListAsync();
    }
}
