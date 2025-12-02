using Microsoft.EntityFrameworkCore;
using TurnoLink.DataAccess.Entities;

namespace TurnoLink.DataAccess.Data;

/// <summary>
/// Contexto de base de datos para TurnoLink
/// Maneja todas las entidades y configuraciones de PostgreSQL
/// </summary>
public class TurnoLinkDbContext : DbContext
{
    public TurnoLinkDbContext(DbContextOptions<TurnoLinkDbContext> options) 
        : base(options)
    {
    }

    // DbSets - Entidades principales
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Service> Services { get; set; } = null!;
    public DbSet<Booking> Bookings { get; set; } = null!;
    public DbSet<Availability> Availabilities { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;

    /// <summary>
    /// Configuración del modelo de datos y relaciones
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();
            
            entity.HasMany(u => u.Services)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.Availabilities)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.Bookings)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuración de Client
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();

            entity.HasMany(c => c.Bookings)
                .WithOne(b => b.Client)
                .HasForeignKey(b => b.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuración de Service
        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasOne(s => s.User)
                .WithMany(u => u.Services)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(s => s.Bookings)
                .WithOne(b => b.Service)
                .HasForeignKey(b => b.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuración de Booking
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasOne(b => b.Client)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.Service)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(b => b.Notifications)
                .WithOne(n => n.Booking)
                .HasForeignKey(n => n.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(b => b.StartTime);
            entity.HasIndex(b => b.Status);
        });

        // Configuración de Availability
        modelBuilder.Entity<Availability>(entity =>
        {
            entity.HasOne(a => a.User)
                .WithMany(u => u.Availabilities)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(a => new { a.UserId, a.DayOfWeek });
        });

        // Configuración de Notification
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasOne(n => n.Booking)
                .WithMany(b => b.Notifications)
                .HasForeignKey(n => n.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(n => n.Status);
        });
    }

    /// <summary>
    /// Sobrescribir SaveChanges para actualizar automáticamente UpdatedAt
    /// </summary>
    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    /// <summary>
    /// Sobrescribir SaveChangesAsync para actualizar automáticamente UpdatedAt
    /// </summary>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Actualiza automáticamente los timestamps de las entidades
    /// </summary>
    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.Entity.GetType().GetProperty("UpdatedAt") != null)
            {
                entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }
        }
    }
}
