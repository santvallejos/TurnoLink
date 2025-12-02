using Microsoft.EntityFrameworkCore;
using TurnoLink.DataAccess.Data;
using TurnoLink.DataAccess.Entities;
using TurnoLink.DataAccess.Interfaces;

namespace TurnoLink.DataAccess.Repositories
{
    /// <summary>
    /// Implementación del repositorio de usuarios
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly TurnoLinkDbContext _context;
        private readonly DbSet<User> _dbSet;

        public UserRepository(TurnoLinkDbContext context)
        {
            _context = context;
            _dbSet = context.Set<User>();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _dbSet
                .Where(u => u.IsActive)
                .ToListAsync();
        }

        public async Task<User> AddAsync(User entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public void Update(User entity)
        {
            _dbSet.Update(entity);
        }

        public void Remove(User entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<bool> ExistsAsync(Func<User, bool> predicate)
        {
            return await Task.Run(() => _dbSet.Any(predicate));
        }
    }
}
