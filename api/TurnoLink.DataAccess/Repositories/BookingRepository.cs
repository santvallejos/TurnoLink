using Microsoft.EntityFrameworkCore;
using TurnoLink.DataAccess.Data;
using TurnoLink.DataAccess.Entities;
using TurnoLink.DataAccess.Interfaces;

namespace TurnoLink.DataAccess.Repositories
{
    /// <summary>
    /// Implementación del repositorio de reservas
    /// </summary>
    public class BookingRepository : IBookingRepository
    {
        private readonly TurnoLinkDbContext _db;

        public BookingRepository(TurnoLinkDbContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _db.Bookings
                .Include(b => b.Client)
                .Include(b => b.Service)
                .Include(b => b.User)
                .ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(Guid id)
        {
            return await _db.Bookings
                .Include(b => b.Client)
                .Include(b => b.Service)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByClientIdAsync(Guid clientId)
        {
            return await _db.Bookings
                .Include(b => b.Service)
                .Include(b => b.User)
                .Where(b => b.ClientId == clientId)
                .OrderByDescending(b => b.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(Guid userId)
        {
            return await _db.Bookings
                .Include(b => b.Client)
                .Include(b => b.Service)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _db.Bookings
                .Include(b => b.Client)
                .Include(b => b.Service)
                .Include(b => b.User)
                .Where(b => b.StartTime >= startDate && b.StartTime <= endDate)
                .OrderBy(b => b.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserAndDateAsync(Guid userId, DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            return await _db.Bookings
                .Include(b => b.Client)
                .Include(b => b.Service)
                .Where(b => b.UserId == userId 
                    && b.StartTime >= startOfDay 
                    && b.StartTime < endOfDay)
                .OrderBy(b => b.StartTime)
                .ToListAsync();
        }

        public async Task<Booking> AddAsync(Booking entity)
        {
            await _db.Bookings.AddAsync(entity);
            return entity;
        }

        public void Update(Booking entity)
        {
            _db.Bookings.Update(entity);
        }

        public void Remove(Booking entity)
        {
            _db.Bookings.Remove(entity);
        }

        public async Task<bool> ExistsAsync(Func<Booking, bool> predicate)
        {
            return await Task.Run(() => _db.Bookings.Any(predicate));
        }
    }
}
