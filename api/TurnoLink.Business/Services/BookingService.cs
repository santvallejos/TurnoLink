using TurnoLink.Business.DTOs;
using TurnoLink.Business.Interfaces;
using TurnoLink.DataAccess.Data;
using TurnoLink.DataAccess.Entities;
using TurnoLink.DataAccess.Enums;
using TurnoLink.DataAccess.Interfaces;

namespace TurnoLink.Business.Services
{
    /// <summary>
    /// Servicio de gestión de reservas/turnos
    /// </summary>
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly TurnoLinkDbContext _context;

        public BookingService(
            IBookingRepository bookingRepository,
            IClientRepository clientRepository,
            IServiceRepository serviceRepository,
            TurnoLinkDbContext context)
        {
            _bookingRepository = bookingRepository;
            _clientRepository = clientRepository;
            _serviceRepository = serviceRepository;
            _context = context;
        }

        public async Task<BookingDto> CreateBookingAsync(CreateBookingDto createBookingDto)
        {
            // Validar que el servicio existe y está activo
            var service = await _serviceRepository.GetByIdAsync(createBookingDto.ServiceId);
            if (service == null || !service.IsActive)
            {
                throw new InvalidOperationException("Servicio no encontrado o no disponible");
            }

            // Buscar o crear cliente
            var client = await _clientRepository.GetByEmailAsync(createBookingDto.ClientEmail);
            if (client == null)
            {
                client = new Client
                {
                    Id = Guid.NewGuid(),
                    FullName = createBookingDto.ClientName,
                    Email = createBookingDto.ClientEmail,
                    PhoneNumber = createBookingDto.ClientPhone,
                    CreatedAt = DateTime.UtcNow
                };
                await _clientRepository.AddAsync(client);
            }

            // Calcular hora de fin basada en duración del servicio
            var endTime = createBookingDto.StartTime.AddMinutes(service.DurationMinutes);

            // Verificar disponibilidad
            var isAvailable = await CheckAvailabilityAsync(service.UserId, createBookingDto.StartTime, service.DurationMinutes);
            if (!isAvailable)
            {
                throw new InvalidOperationException("El horario seleccionado no está disponible");
            }

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                ClientId = client.Id,
                ServiceId = service.Id,
                UserId = service.UserId,
                StartTime = createBookingDto.StartTime,
                EndTime = endTime,
                Status = BookingStatus.Pending,
                Notes = createBookingDto.Notes,
                CreatedAt = DateTime.UtcNow
            };

            await _bookingRepository.AddAsync(booking);
            await _context.SaveChangesAsync();

            // Recargar con relaciones
            var createdBooking = await _bookingRepository.GetByIdAsync(booking.Id);
            return MapToDto(createdBooking!);
        }

        public async Task<BookingDto?> GetBookingByIdAsync(Guid id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            return booking == null ? null : MapToDto(booking);
        }

        public async Task<IEnumerable<BookingDto>> GetBookingsByClientIdAsync(Guid clientId)
        {
            var bookings = await _bookingRepository.GetBookingsByClientIdAsync(clientId);
            return bookings.Select(MapToDto);
        }

        public async Task<IEnumerable<BookingDto>> GetBookingsByUserIdAsync(Guid userId)
        {
            var bookings = await _bookingRepository.GetBookingsByUserIdAsync(userId);
            return bookings.Select(MapToDto);
        }

        public async Task<IEnumerable<BookingDto>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var bookings = await _bookingRepository.GetBookingsByDateRangeAsync(startDate, endDate);
            return bookings.Select(MapToDto);
        }

        public async Task<BookingDto> UpdateBookingAsync(Guid id, UpdateBookingDto updateBookingDto)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                throw new InvalidOperationException("Reserva no encontrada");
            }

            if (!string.IsNullOrWhiteSpace(updateBookingDto.Status))
            {
                if (Enum.TryParse<BookingStatus>(updateBookingDto.Status, true, out var status))
                {
                    booking.Status = status;
                }
            }

            if (!string.IsNullOrWhiteSpace(updateBookingDto.Notes))
                booking.Notes = updateBookingDto.Notes;

            _bookingRepository.Update(booking);
            await _context.SaveChangesAsync();

            var updatedBooking = await _bookingRepository.GetByIdAsync(id);
            return MapToDto(updatedBooking!);
        }

        public async Task<bool> CancelBookingAsync(Guid id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                throw new InvalidOperationException("Reserva no encontrada");
            }

            booking.Status = BookingStatus.Canceled;
            _bookingRepository.Update(booking);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CheckAvailabilityAsync(Guid userId, DateTime startTime, int durationMinutes)
        {
            var endTime = startTime.AddMinutes(durationMinutes);
            var date = startTime.Date;

            var existingBookings = await _bookingRepository.GetBookingsByUserAndDateAsync(userId, date);
            
            // Verificar si hay conflictos con reservas existentes (excluyendo canceladas)
            var hasConflict = existingBookings
                .Where(b => b.Status != BookingStatus.Canceled)
                .Any(b => b.StartTime < endTime && b.EndTime > startTime);

            return !hasConflict;
        }

        private static BookingDto MapToDto(Booking booking)
        {
            return new BookingDto
            {
                Id = booking.Id,
                ClientId = booking.ClientId,
                ClientName = booking.Client?.FullName ?? string.Empty,
                ClientEmail = booking.Client?.Email ?? string.Empty,
                ClientPhone = booking.Client?.PhoneNumber,
                ServiceId = booking.ServiceId,
                ServiceName = booking.Service?.Name ?? string.Empty,
                ServicePrice = booking.Service?.Price ?? 0,
                UserId = booking.UserId,
                UserName = booking.User?.FullName ?? string.Empty,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Status = booking.Status.ToString(),
                Notes = booking.Notes,
                CreatedAt = booking.CreatedAt
            };
        }
    }
}
