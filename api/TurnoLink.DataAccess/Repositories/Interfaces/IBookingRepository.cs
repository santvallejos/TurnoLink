using TurnoLink.DataAccess.Entities;

namespace TurnoLink.DataAccess.Repositories.Interfaces;

/// <summary>
/// Interfaz específica para repositorio de reservas
/// </summary>
public interface IBookingRepository : IRepository<Booking>
{
    /// <summary>
    /// Obtiene reservas de un cliente
    /// </summary>
    Task<IEnumerable<Booking>> GetBookingsByClientIdAsync(Guid clientId);

    /// <summary>
    /// Obtiene reservas de un profesional
    /// </summary>
    Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(Guid userId);

    /// <summary>
    /// Obtiene reservas por rango de fechas
    /// </summary>
    Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Obtiene reservas de un profesional por fecha
    /// </summary>
    Task<IEnumerable<Booking>> GetBookingsByUserAndDateAsync(Guid userId, DateTime date);

    /// <summary>
    /// Verifica si hay conflicto de horarios para una reserva
    /// </summary>
    Task<bool> HasTimeConflictAsync(Guid userId, DateTime startTime, DateTime endTime, Guid? excludeBookingId = null);
}
