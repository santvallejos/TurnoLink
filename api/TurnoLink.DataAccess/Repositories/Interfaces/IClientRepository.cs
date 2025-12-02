using TurnoLink.DataAccess.Entities;

namespace TurnoLink.DataAccess.Repositories.Interfaces;

/// <summary>
/// Interfaz específica para repositorio de clientes
/// </summary>
public interface IClientRepository : IRepository<Client>
{
    /// <summary>
    /// Obtiene un cliente por su email
    /// </summary>
    Task<Client?> GetByEmailAsync(string email);

    /// <summary>
    /// Obtiene un cliente por su número de teléfono
    /// </summary>
    Task<Client?> GetByPhoneAsync(string phoneNumber);
}
