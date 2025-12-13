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
        private readonly TurnoLinkDbContext _db;

        public ServiceRepository(TurnoLinkDbContext context)
        {
            _db = context;
        }

        public async Task<Service?> GetByIdAsync(Guid id)
        {
            return await _db.Services
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Service>> GetServicesBySlug(string slug)
        {
            return await _db.Services
                .Where(s => s.User.Slug == slug)
                .ToListAsync();
        }

        public async Task<IEnumerable<Service>> GetServicesByUserIdAsync(Guid userId)
        {
            return await _db.Services
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        public async Task<Service> AddAsync(Service entity)
        {
            await _db.Services.AddAsync(entity);
            return entity;
        }

        public void Update(Service entity)
        {
            _db.Services.Update(entity);
        }

        public void Remove(Service entity)
        {
            _db.Services.Remove(entity);
        }

        public async Task<bool> ExistsAsync(Func<Service, bool> predicate)
        {
            return await Task.Run(() => _db.Services.Any(predicate));
        }
    }
}
