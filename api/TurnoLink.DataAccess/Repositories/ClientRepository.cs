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
        private readonly TurnoLinkDbContext _db;

        public ClientRepository(TurnoLinkDbContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _db.Clients.ToListAsync();
        }

        public async Task<Client?> GetByIdAsync(Guid id)
        {
            return await _db.Clients.FindAsync(id);
        }

        public async Task<Client?> GetByEmailAsync(string email)
        {
            return await _db.Clients
                .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
        }

        public async Task<Client?> GetByPhoneAsync(string phoneNumber)
        {
            return await _db.Clients
                .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber);
        }

        public async Task<Client> AddAsync(Client entity)
        {
            await _db.Clients.AddAsync(entity);
            return entity;
        }

        public void Update(Client entity)
        {
            _db.Clients.Update(entity);
        }

        public void Remove(Client entity)
        {
            _db.Clients.Remove(entity);
        }

        public async Task<bool> ExistsAsync(Func<Client, bool> predicate)
        {
            return await Task.Run(() => _db.Clients.Any(predicate));
        }
    }
}
