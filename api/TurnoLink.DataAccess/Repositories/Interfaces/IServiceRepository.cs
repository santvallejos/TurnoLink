using TurnoLink.DataAccess.Entities;

namespace TurnoLink.DataAccess.Repositories.Interfaces;

/// <summary>
/// Interfaz específica para repositorio de servicios
/// </summary>
public interface IServiceRepository : IRepository<Service>
{
    /// <summary>
    /// Obtiene servicios activos de un profesional
    /// </summary>
    Task<IEnumerable<Service>> GetActiveServicesByUserIdAsync(Guid userId);

    /// <summary>
    /// Obtiene todos los servicios de un profesional
    /// </summary>
    Task<IEnumerable<Service>> GetServicesByUserIdAsync(Guid userId);
}
