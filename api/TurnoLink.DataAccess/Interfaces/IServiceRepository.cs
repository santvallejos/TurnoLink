using TurnoLink.DataAccess.Entities;

namespace TurnoLink.DataAccess.Interfaces
{
    /// <summary>
    /// Interfaz para repositorio de servicios
    /// </summary>
    public interface IServiceRepository
    {
        /// <summary>
        /// Obtiene todos los servicios
        /// </summary>
        Task<IEnumerable<Service>> GetAllAsync();

        /// <summary>
        /// Obtiene un servicio por su ID
        /// </summary>
        Task<Service?> GetByIdAsync(Guid id);

        /// <summary>
        /// Obtiene servicios activos por ID de usuario
        /// </summary>
        Task<IEnumerable<Service>> GetActiveServicesByUserIdAsync(Guid userId);

        /// <summary>
        /// Obtiene todos los servicios por ID de usuario
        /// </summary>
        Task<IEnumerable<Service>> GetServicesByUserIdAsync(Guid userId);

        /// <summary>
        /// Agrega un nuevo servicio
        /// </summary>
        Task<Service> AddAsync(Service entity);

        /// <summary>
        /// Actualiza un servicio existente
        /// </summary>
        void Update(Service entity);

        /// <summary>
        /// Elimina un servicio
        /// </summary>
        void Remove(Service entity);

        /// <summary>
        /// Verifica si existe un servicio
        /// </summary>
        Task<bool> ExistsAsync(Func<Service, bool> predicate);
    }
}