using TurnoLink.DataAccess.Entities;

namespace TurnoLink.DataAccess.Interfaces
{
    /// <summary>
    /// Interfaz para repositorio de clientes
    /// </summary>
    public interface IClientRepository
    {
        /// <summary>
        /// Obtiene todos los clientes
        /// </summary>
        Task<IEnumerable<Client>> GetAllAsync();

        /// <summary>
        /// Obtiene un cliente por su ID
        /// </summary>
        Task<Client?> GetByIdAsync(Guid id);

        /// <summary>
        /// Obtiene un cliente por su email
        /// </summary>
        Task<Client?> GetByEmailAsync(string email);

        /// <summary>
        /// Obtiene un cliente por su teléfono
        /// </summary>
        Task<Client?> GetByPhoneAsync(string phoneNumber);

        /// <summary>
        /// Agrega un nuevo cliente
        /// </summary>
        Task<Client> AddAsync(Client entity);

        /// <summary>
        /// Actualiza un cliente existente
        /// </summary>
        void Update(Client entity);

        /// <summary>
        /// Elimina un cliente
        /// </summary>
        void Remove(Client entity);

        /// <summary>
        /// Verifica si existe un cliente
        /// </summary>
        Task<bool> ExistsAsync(Func<Client, bool> predicate);
    }
}