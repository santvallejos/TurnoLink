using Microsoft.EntityFrameworkCore;
using TurnoLink.DataAccess.Data;
using TurnoLink.DataAccess.Entities;
using TurnoLink.DataAccess.Interfaces;

namespace TurnoLink.DataAccess.Repositories
{
    /// <summary>
    /// Implementation of the availability repository
    /// </summary>
    public class AvailabilityRepository : IAvailabilityRepository
    {
        private readonly TurnoLinkDbContext _context;
        private readonly DbSet<Availability> _dbSet;
        
        /// <summary>
        /// Constructor for AvailabilityRepository
        /// </summary>
        /// <param name="context">Database context</param>
        public AvailabilityRepository(TurnoLinkDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Availability>();
        }

        /// <summary>
        /// Get availability entity by ID
        /// </summary>
        /// <param name="id">Availability ID</param>
        /// <returns>Availability entity or null if not found</returns>
        public async Task<Availability?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Gets availabilities by user ID
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>Collection of availabilities for the user</returns>
        public async Task<IEnumerable<Availability>> GetAvailabilitiesByUserIdAsync(Guid userId)
        {
            return await _dbSet
                .Where(a => a.UserId == userId)
                .OrderBy(a => a.StartTime)
                .ToListAsync();
        }

        /// <summary>
        /// Gets availabilities by service ID
        /// </summary>
        /// <param name="serviceId">Service ID</param>
        /// <returns>Collection of availabilities for the service</returns>
        public async Task<IEnumerable<Availability>> GetAvailabilitiesByServiceIdAsync(Guid serviceId)
        {
            return await _dbSet
                .Where(a => a.ServiceId == serviceId)
                .OrderBy(a => a.StartTime)
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new availability
        /// </summary>
        /// <param name="entity">Availability entity to add</param>
        /// <returns>Added availability entity</returns>
        public async Task<Availability> AddAsync(Availability entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Updates an existing availability
        /// </summary>
        /// <param name="entity">Availability entity to update</param>
        public void Update(Availability entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Removes an availability
        /// </summary>
        /// <param name="entity">Availability entity to remove</param>
        public void Remove(Availability entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Checks if an availability exists that meets a condition
        /// </summary>
        /// <param name="predicate">Condition to check</param>
        /// <returns>True if exists, false otherwise</returns>
        public async Task<bool> ExistsAsync(Func<Availability, bool> predicate)
        {
            return await Task.Run(() => _dbSet.Any(predicate));
        }
    }
}