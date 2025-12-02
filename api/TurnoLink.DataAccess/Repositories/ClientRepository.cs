using Microsoft.EntityFrameworkCore;
using TurnoLink.DataAccess.Data;
using TurnoLink.DataAccess.Entities;
using TurnoLink.DataAccess.Repositories.Interfaces;

namespace TurnoLink.DataAccess.Repositories;

/// <summary>
/// Implementación del repositorio de clientes
/// </summary>
public class ClientRepository : Repository<Client>, IClientRepository
{
    public ClientRepository(TurnoLinkDbContext context) : base(context)
    {
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
}
