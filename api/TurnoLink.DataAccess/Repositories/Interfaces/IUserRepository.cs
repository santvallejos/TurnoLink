using TurnoLink.DataAccess.Entities;

namespace TurnoLink.DataAccess.Repositories.Interfaces;

/// <summary>
/// Interfaz específica para repositorio de usuarios
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Obtiene un usuario por su email
    /// </summary>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Obtiene usuarios activos
    /// </summary>
    Task<IEnumerable<User>> GetActiveUsersAsync();
}
