namespace TurnoLink.Business.DTOs
{
    public class CreateBookingDto
    {
        public Guid ServiceId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ClientEmail { get; set; } = string.Empty;
        public string? ClientPhone { get; set; }
        public DateTime StartTime { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateBookingDto
    {
        public DateTime? StartTime { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
    }

    public class BookingDto
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string? ClientEmail { get; set; }
        public string? ClientPhone { get; set; }
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public decimal ServicePrice { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}