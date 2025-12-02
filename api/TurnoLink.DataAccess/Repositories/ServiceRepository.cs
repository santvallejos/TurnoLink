using Microsoft.EntityFrameworkCore;
using TurnoLink.DataAccess.Data;
using TurnoLink.DataAccess.Entities;
using TurnoLink.DataAccess.Repositories.Interfaces;

namespace TurnoLink.DataAccess.Repositories;

/// <summary>
/// Implementación del repositorio de servicios
/// </summary>
public class ServiceRepository : Repository<Service>, IServiceRepository
{
    public ServiceRepository(TurnoLinkDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Service>> GetActiveServicesByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Where(s => s.UserId == userId && s.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Service>> GetServicesByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Where(s => s.UserId == userId)
            .ToListAsync();
    }

    public override async Task<Service?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}
