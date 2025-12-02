using TurnoLink.DataAccess.Entities;

namespace TurnoLink.DataAccess.Interfaces
{
    /// <summary>
    /// Interfaz para repositorio de reservas
    /// </summary>
    public interface IBookingRepository
    {
        /// <summary>
        /// Obtiene todas las reservas
        /// </summary>
        Task<IEnumerable<Booking>> GetAllAsync();

        /// <summary>
        /// Obtiene una reserva por su ID
        /// </summary>
        Task<Booking?> GetByIdAsync(Guid id);

        /// <summary>
        /// Obtiene reservas por ID de cliente
        /// </summary>
        Task<IEnumerable<Booking>> GetBookingsByClientIdAsync(Guid clientId);

        /// <summary>
        /// Obtiene reservas por ID de usuario
        /// </summary>
        Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(Guid userId);

        /// <summary>
        /// Obtiene reservas por rango de fechas
        /// </summary>
        Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Obtiene reservas por usuario y fecha
        /// </summary>
        Task<IEnumerable<Booking>> GetBookingsByUserAndDateAsync(Guid userId, DateTime date);

        /// <summary>
        /// Agrega una nueva reserva
        /// </summary>
        Task<Booking> AddAsync(Booking entity);

        /// <summary>
        /// Actualiza una reserva existente
        /// </summary>
        void Update(Booking entity);

        /// <summary>
        /// Elimina una reserva
        /// </summary>
        void Remove(Booking entity);

        /// <summary>
        /// Verifica si existe una reserva
        /// </summary>
        Task<bool> ExistsAsync(Func<Booking, bool> predicate);
    }
}