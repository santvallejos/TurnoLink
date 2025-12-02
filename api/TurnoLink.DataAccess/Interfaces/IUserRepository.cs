using TurnoLink.DataAccess.Entities;

namespace TurnoLink.DataAccess.Interfaces
{
    /// <summary>
    /// Interfaz específica para repositorio de usuarios
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Obtiene todas las entidades
        /// </summary>
        Task<IEnumerable<User>> GetAllAsync();

        /// <summary>
        /// Obtiene un usuario por su ID
        /// </summary>
        Task<User?> GetByIdAsync(Guid id);

        /// <summary>
        /// Obtiene un usuario por su email
        /// </summary>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Obtiene usuarios activos
        /// </summary>
        Task<IEnumerable<User>> GetActiveUsersAsync();

        /// <summary>
        /// Agrega un nuevo usuario
        /// </summary>
        Task<User> AddAsync(User entity);

        /// <summary>
        /// Actualiza un usuario existente
        /// </summary>
        void Update(User entity);

        /// <summary>
        /// Elimina un usuario
        /// </summary>
        void Remove(User entity);

        /// <summary>
        /// Verifica si existe un usuario que cumpla una condición
        /// </summary>
        Task<bool> ExistsAsync(Func<User, bool> predicate);
    }
}
