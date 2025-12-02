namespace TurnoLink.DataAccess.Repositories.Interfaces;

/// <summary>
/// Interfaz para Unit of Work pattern
/// Coordina el trabajo de múltiples repositorios y maneja transacciones
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Repositorio de usuarios
    /// </summary>
    IUserRepository Users { get; }

    /// <summary>
    /// Repositorio de clientes
    /// </summary>
    IClientRepository Clients { get; }

    /// <summary>
    /// Repositorio de servicios
    /// </summary>
    IServiceRepository Services { get; }

    /// <summary>
    /// Repositorio de reservas
    /// </summary>
    IBookingRepository Bookings { get; }

    /// <summary>
    /// Repositorio de disponibilidades
    /// </summary>
    IAvailabilityRepository Availabilities { get; }

    /// <summary>
    /// Repositorio de notificaciones
    /// </summary>
    INotificationRepository Notifications { get; }

    /// <summary>
    /// Guarda todos los cambios pendientes en la base de datos
    /// </summary>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Inicia una transacción de base de datos
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// Confirma la transacción actual
    /// </summary>
    Task CommitTransactionAsync();

    /// <summary>
    /// Revierte la transacción actual
    /// </summary>
    Task RollbackTransactionAsync();
}
