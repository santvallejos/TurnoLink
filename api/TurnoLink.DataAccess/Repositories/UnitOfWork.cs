using Microsoft.EntityFrameworkCore.Storage;
using TurnoLink.DataAccess.Data;
using TurnoLink.DataAccess.Repositories.Interfaces;

namespace TurnoLink.DataAccess.Repositories;

/// <summary>
/// Implementación del patrón Unit of Work
/// Coordina el trabajo de múltiples repositorios y maneja transacciones
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly TurnoLinkDbContext _context;
    private IDbContextTransaction? _transaction;

    private IUserRepository? _users;
    private IClientRepository? _clients;
    private IServiceRepository? _services;
    private IBookingRepository? _bookings;
    private IAvailabilityRepository? _availabilities;
    private INotificationRepository? _notifications;

    public UnitOfWork(TurnoLinkDbContext context)
    {
        _context = context;
    }

    public IUserRepository Users => _users ??= new UserRepository(_context);

    public IClientRepository Clients => _clients ??= new ClientRepository(_context);

    public IServiceRepository Services => _services ??= new ServiceRepository(_context);

    public IBookingRepository Bookings => _bookings ??= new BookingRepository(_context);

    public IAvailabilityRepository Availabilities => _availabilities ??= new AvailabilityRepository(_context);

    public INotificationRepository Notifications => _notifications ??= new NotificationRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
