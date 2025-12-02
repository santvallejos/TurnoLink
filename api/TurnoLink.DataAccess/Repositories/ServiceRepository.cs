using Microsoft.EntityFrameworkCore;
using TurnoLink.DataAccess.Data;
using TurnoLink.DataAccess.Entities;
using TurnoLink.DataAccess.Interfaces;

namespace TurnoLink.DataAccess.Repositories
{
    /// <summary>
    /// Implementación del repositorio de servicios
    /// </summary>
    public class ServiceRepository : IServiceRepository
    {
        private readonly TurnoLinkDbContext _context;
        private readonly DbSet<Service> _dbSet;

        public ServiceRepository(TurnoLinkDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Service>();
        }

        public async Task<IEnumerable<Service>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Service?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
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

        public async Task<Service> AddAsync(Service entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public void Update(Service entity)
        {
            _dbSet.Update(entity);
        }

        public void Remove(Service entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<bool> ExistsAsync(Func<Service, bool> predicate)
        {
            return await Task.Run(() => _dbSet.Any(predicate));
        }
    }
}
