using Microsoft.EntityFrameworkCore;
using TurnoLink.DataAccess.Data;
using TurnoLink.DataAccess.Entities;
using TurnoLink.DataAccess.Interfaces;

namespace TurnoLink.DataAccess.Repositories
{
    /// <summary>
    /// Repository for managing Availability entities
    /// </summary>
    public class AvailabilityRepository : IAvailabilityRepository
    {
        private readonly TurnoLinkDbContext _db;

        public AvailabilityRepository(TurnoLinkDbContext context)
        {
            _db = context;
        }

        /// <summary>
        /// Gets an availability by ID
        /// </summary>
        public async Task<Availability?> GetByIdAsync(Guid id)
        {
            return await _db.Availabilities.FindAsync(id);
        }

        /// <summary>
        /// Gets all availabilities for a specific user, ordered by start time (UTC)
        /// </summary>
        public async Task<IEnumerable<Availability>> GetAvailabilitiesByUserIdAsync(Guid userId)
        {
            return await _db.Availabilities
                .Where(a => a.UserId == userId)
                .OrderBy(a => a.StartTimeUtc)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all availabilities for a specific service, ordered by start time (UTC)
        /// </summary>
        public async Task<IEnumerable<Availability>> GetAvailabilitiesByServiceIdAsync(Guid serviceId)
        {
            return await _db.Availabilities
                .Where(a => a.ServiceId == serviceId)
                .OrderBy(a => a.StartTimeUtc)
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new availability to the database
        /// </summary>
        public async Task<Availability> AddAsync(Availability entity)
        {
            await _db.Availabilities.AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// Updates an existing availability
        /// </summary>
        public void Update(Availability entity)
        {
            _db.Availabilities.Update(entity);
        }

        /// <summary>
        /// Removes an availability from the database
        /// </summary>
        public void Remove(Availability entity)
        {
            _db.Availabilities.Remove(entity);
        }

        /// <summary>
        /// Checks if any availability matches the given predicate
        /// </summary>
        public async Task<bool> ExistsAsync(Func<Availability, bool> predicate)
        {
            return await Task.Run(() => _db.Availabilities.Any(predicate));
        }
    }
}