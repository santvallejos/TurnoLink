namespace TurnoLink.Business.DTOs
{
    public class BookingNotificationDto
    {
        public string Type { get; set; } = "NewBooking";

        public Guid BookingId { get; set; }

        public string ClientName { get; set; } = string.Empty;

        public string ServiceName { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Message { get; set; } = string.Empty;
    }
}
