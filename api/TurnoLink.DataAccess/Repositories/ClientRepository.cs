using Microsoft.EntityFrameworkCore;
using TurnoLink.DataAccess.Data;
using TurnoLink.DataAccess.Entities;
using TurnoLink.DataAccess.Interfaces;

namespace TurnoLink.DataAccess.Repositories
{
    /// <summary>
    /// Implementación del repositorio de clientes
    /// </summary>
    public class ClientRepository : IClientRepository
    {
        private readonly TurnoLinkDbContext _context;
        private readonly DbSet<Client> _dbSet;

        public ClientRepository(TurnoLinkDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Client>();
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Client?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<Client?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
        }

        public async Task<Client?> GetByPhoneAsync(string phoneNumber)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber);
        }

        public async Task<Client> AddAsync(Client entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public void Update(Client entity)
        {
            _dbSet.Update(entity);
        }

        public void Remove(Client entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<bool> ExistsAsync(Func<Client, bool> predicate)
        {
            return await Task.Run(() => _dbSet.Any(predicate));
        }
    }
}
