using TurnoLink.Business.DTOs;
using TurnoLink.Business.Interfaces;
using TurnoLink.DataAccess.Data;
using TurnoLink.DataAccess.Entities;
using TurnoLink.DataAccess.Enums;
using TurnoLink.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TurnoLink.Business.Services
{
    /// <summary>
    /// Service to manage bookings.
    /// </summary>
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IiCalDotnet _serviceIcalDotnet;
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly TurnoLinkDbContext _context;
        private readonly ResendService _resendService;

        /// <summary>
        /// Constructor for BookingService.
        /// </summary>
        /// <param name="bookingRepository">Booking repository</param>
        /// <param name="clientRepository">Client repository</param>
        /// <param name="serviceRepository">Service repository</param>
        /// <param name="context">Database context</param>
        public BookingService(
            IBookingRepository bookingRepository,
            IClientRepository clientRepository,
            IServiceRepository serviceRepository,
            IiCalDotnet serviceIcalDotnet,
            IAvailabilityRepository availabilityRepository,
            ResendService resendService,
            TurnoLinkDbContext context)
        {
            _bookingRepository = bookingRepository;
            _clientRepository = clientRepository;
            _serviceRepository = serviceRepository;
            _availabilityRepository = availabilityRepository;
            _serviceIcalDotnet = serviceIcalDotnet;
            _resendService = resendService;
            _context = context;
        }

        public async Task<BookingDto?> GetBookingByIdAsync(Guid id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);

            if (booking == null)
                throw new InvalidOperationException("Booking not found");

            return MapToDto(booking);
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

        public async Task<BookingDto> CreateBookingAsync(CreateBookingDto createBookingDto)
        {
            // Validate that the service exists and is active
            var service = await _serviceRepository.GetByIdAsync(createBookingDto.ServiceId);
            if (service == null || !service.IsActive)
                throw new InvalidOperationException("Service not found or not available");

            var availability = await _availabilityRepository.GetByIdAsync(createBookingDto.AvailabilityId);
            if (availability == null)
                throw new InvalidOperationException("Availability not found");
            
            // Validate that the availability is not in the past
            if (availability.StartTime < DateTime.UtcNow)
                throw new InvalidOperationException("Cannot book an appointment in the past. Please select a future date.");
            
            if (availability.UserId != service.UserId)
                throw new InvalidOperationException("Availability does not match the service's user");

            // Validate that the availability is not already booked
            var existingBooking = await _context.Bookings
                .Where(b => b.AvailabilityId == createBookingDto.AvailabilityId 
                    && b.Status != BookingStatus.Canceled 
                    && b.Status != BookingStatus.NoShow)
                .FirstOrDefaultAsync();

            if (existingBooking != null)
                throw new InvalidOperationException("This time slot is already booked. Please select another availability.");

            // Find or create client
            var client = await _clientRepository.GetByEmailAsync(createBookingDto.ClientEmail);
            if (client == null)
            {
                client = new Client
                {
                    Id = Guid.NewGuid(),
                    Name = createBookingDto.ClientName,
                    Surname = createBookingDto.ClientSurname,
                    Email = createBookingDto.ClientEmail,
                    PhoneNumber = createBookingDto.ClientPhone,
                    CreatedAt = DateTime.UtcNow
                };
                await _clientRepository.AddAsync(client);
                await _context.SaveChangesAsync();
            }

            // Calculate end time based on service duration
            var endTime = availability.StartTime.AddMinutes(service.DurationMinutes);

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                ClientId = client.Id,
                ServiceId = service.Id,
                UserId = service.UserId,
                AvailabilityId = createBookingDto.AvailabilityId,
                StartTime = availability.StartTime,
                EndTime = endTime,
                Status = BookingStatus.Pending,
                Notes = createBookingDto.Notes,
                CreatedAt = DateTime.UtcNow
            };

            await _bookingRepository.AddAsync(booking);
            await _context.SaveChangesAsync();

            // Reload with relationships
            var createdBooking = await _bookingRepository.GetByIdAsync(booking.Id);
            if (createdBooking == null)
                throw new InvalidOperationException("Error creating booking");
            
            var bookingDto = MapToDto(createdBooking);
            
            // Send confirmation email to client
            var icsContent = await _serviceIcalDotnet.CreateFileIcsBookingAsync(bookingDto);
            await _resendService.SendClientConfirmationEmailAsync(bookingDto, icsContent);
            
            // Send notification email to professional
            if (!string.IsNullOrEmpty(bookingDto.UserEmail))
            {
                await _resendService.SendProfessionalNotificationEmailAsync(bookingDto, bookingDto.UserEmail);
            }
            
            return bookingDto;
        }

        public async Task<BookingDto> UpdateBookingAsync(Guid id, UpdateBookingDto updateBookingDto)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
                throw new InvalidOperationException("Booking not found");

            if (!string.IsNullOrWhiteSpace(updateBookingDto.Status))
            {
                if (Enum.TryParse<BookingStatus>(updateBookingDto.Status, true, out var status))
                    booking.Status = status;
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
                throw new InvalidOperationException("Booking not found");

            booking.Status = BookingStatus.Canceled;
            _bookingRepository.Update(booking);
            await _context.SaveChangesAsync();

            return true;
        }

        private static BookingDto MapToDto(Booking booking)
        {
            return new BookingDto
            {
                Id = booking.Id,
                ClientName = $"{booking.Client?.Name} {booking.Client?.Surname}" ?? string.Empty,
                ClientEmail = booking.Client?.Email ?? string.Empty,
                ClientPhone = booking.Client?.PhoneNumber,
                ServiceId = booking.ServiceId,
                UserId = booking.UserId,
                ServiceName = booking.Service?.Name ?? string.Empty,
                ServicePrice = booking.Service?.Price ?? 0,
                UserName = $"{booking.User?.Name} {booking.User?.Surname}" ?? string.Empty,
                UserEmail = booking.User?.Email,
                UserPhone = booking.User?.PhoneNumber,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Status = booking.Status.ToString(),
                Notes = booking.Notes,
                CreatedAt = booking.CreatedAt,
                Location = booking.User?.Address
            };
        }
    }
}
