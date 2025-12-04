using Microsoft.EntityFrameworkCore;
using TurnoLink.DataAccess.Entities;

namespace TurnoLink.DataAccess.Data
{
    /// <summary>
    /// Database context of TurnoLink.
    /// Manages all entities and PostgreSQL configurations.
    /// </summary>
    public class TurnoLinkDbContext : DbContext
    {
        public TurnoLinkDbContext(DbContextOptions<TurnoLinkDbContext> options)
            : base(options)
        {
        }

        // DbSets - Main entities
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Service> Services { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<Availability> Availabilities { get; set; } = null!;
        /* public DbSet<Notification> Notifications { get; set; } = null!; */

        /// <summary>
        /// Configuration of the data model and relationships
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration of User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();

                entity.HasMany(u => u.Services)
                    .WithOne(s => s.User)
                    .HasForeignKey(s => s.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.Bookings)
                    .WithOne(b => b.User)
                    .HasForeignKey(b => b.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuration of Client
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();

                entity.HasMany(c => c.Bookings)
                    .WithOne(b => b.Client)
                    .HasForeignKey(b => b.ClientId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuration of Service
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

            // Configuration of Booking
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

                /* entity.HasMany(b => b.Notifications)
                    .WithOne(n => n.Booking)
                    .HasForeignKey(n => n.BookingId)
                    .OnDelete(DeleteBehavior.Cascade); */

                entity.HasIndex(b => b.StartTime);
                entity.HasIndex(b => b.Status);
            });

            // Configuration of Availability
            modelBuilder.Entity<Availability>(entity =>
            {
                entity.HasIndex(a => a.UserId);
                entity.HasIndex(a => a.ServiceId);
                entity.HasIndex(a => a.StartTime);
            });

            // Configuration of Notification
            /* modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasOne(n => n.Booking)
                    .WithMany(b => b.Notifications)
                    .HasForeignKey(n => n.BookingId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(n => n.Status);
            }); */
        }

        /// <summary>
        /// Override SaveChanges to automatically update UpdatedAt
        /// </summary>
        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        /// <summary>
        /// Override SaveChangesAsync to automatically update UpdatedAt
        /// </summary>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Automatically updates the timestamps of entities
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
}