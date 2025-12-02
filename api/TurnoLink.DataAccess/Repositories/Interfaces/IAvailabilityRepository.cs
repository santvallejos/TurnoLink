using TurnoLink.DataAccess.Entities;

namespace TurnoLink.DataAccess.Repositories.Interfaces;

/// <summary>
/// Interfaz específica para repositorio de disponibilidades
/// </summary>
public interface IAvailabilityRepository : IRepository<Availability>
{
    /// <summary>
    /// Obtiene disponibilidades activas de un profesional
    /// </summary>
    Task<IEnumerable<Availability>> GetActiveAvailabilitiesByUserIdAsync(Guid userId);

    /// <summary>
    /// Obtiene disponibilidades de un profesional para un día específico
    /// </summary>
    Task<IEnumerable<Availability>> GetAvailabilitiesByUserAndDayAsync(Guid userId, int dayOfWeek);
}
