using Microsoft.EntityFrameworkCore;
using TurnoLink.DataAccess.Data;
using TurnoLink.DataAccess.Entities;
using TurnoLink.DataAccess.Interfaces;

namespace TurnoLink.DataAccess.Repositories
{
    public class AvailabilityRepository : IAvailabilityRepository
    {
        private readonly TurnoLinkDbContext _db;

        public AvailabilityRepository(TurnoLinkDbContext context)
        {
            _db = context;
        }
        public async Task<Availability?> GetByIdAsync(Guid id)
        {
            return await _db.Availabilities.FindAsync(id);
        }

        public async Task<IEnumerable<Availability>> GetAvailabilitiesByUserIdAsync(Guid userId)
        {
            return await _db.Availabilities
                .Where(a => a.UserId == userId)
                .OrderBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Availability>> GetAvailabilitiesByServiceIdAsync(Guid serviceId)
        {
            return await _db.Availabilities
                .Where(a => a.ServiceId == serviceId)
                .OrderBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<Availability> AddAsync(Availability entity)
        {
            await _db.Availabilities.AddAsync(entity);
            return entity;
        }

        public void Update(Availability entity)
        {
            _db.Availabilities.Update(entity);
        }

        public void Remove(Availability entity)
        {
            _db.Availabilities.Remove(entity);
        }

        public async Task<bool> ExistsAsync(Func<Availability, bool> predicate)
        {
            return await Task.Run(() => _db.Availabilities.Any(predicate));
        }
    }
}