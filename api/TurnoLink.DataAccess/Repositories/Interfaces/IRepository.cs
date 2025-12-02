using System.Linq.Expressions;

namespace TurnoLink.DataAccess.Repositories.Interfaces;

/// <summary>
/// Interfaz genérica para repositorios con operaciones CRUD básicas
/// </summary>
/// <typeparam name="T">Tipo de entidad</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Obtiene todas las entidades
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Obtiene una entidad por su ID
    /// </summary>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Busca entidades que cumplan una condición
    /// </summary>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Agrega una nueva entidad
    /// </summary>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Agrega múltiples entidades
    /// </summary>
    Task AddRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// Actualiza una entidad existente
    /// </summary>
    void Update(T entity);

    /// <summary>
    /// Elimina una entidad
    /// </summary>
    void Remove(T entity);

    /// <summary>
    /// Elimina múltiples entidades
    /// </summary>
    void RemoveRange(IEnumerable<T> entities);

    /// <summary>
    /// Verifica si existe una entidad que cumpla una condición
    /// </summary>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Cuenta entidades que cumplan una condición
    /// </summary>
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);
}
