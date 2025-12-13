namespace TurnoLink.Business.DTOs
{
    public class CreateBookingDto
    {
        public Guid ServiceId { get; set; }
        public Guid AvailabilityId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ClientSurname { get; set; } = string.Empty;
        public string ClientEmail { get; set; } = string.Empty;
        public string? ClientPhone { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateBookingDto
    {
        public Guid? AvailabilityId { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
    }


    public class BookingDto
    {
        public Guid Id { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ClientSurname { get; set; } = string.Empty;
        public string? ClientEmail { get; set; }
        public string? ClientPhone { get; set; }
        public Guid UserId { get; set; }
        public Guid ServiceId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public decimal ServicePrice { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}