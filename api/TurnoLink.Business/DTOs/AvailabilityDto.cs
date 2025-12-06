using System.ComponentModel.DataAnnotations;

namespace TurnoLink.Business.DTOs
{
    public class CreateAvailabilityDto
    {
        [Required(ErrorMessage = "Service ID is required")]
        public Guid ServiceId { get; set; }

        [Required(ErrorMessage = "Start date and time is required")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [Range(15, 480, ErrorMessage = "Duration must be between 15 minutes and 8 hours")]
        public int DurationMinutes { get; set; }
    }

    public class UpdateAvailabilityDto
    {
        public DateTime? StartTime { get; set; }

        [Range(15, 480, ErrorMessage = "Duration must be between 15 minutes and 8 hours")]
        public int? DurationMinutes { get; set; }
    }

    public class AvailabilityDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ServiceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int DurationMinutes { get; set; }
        public string ServiceName { get; set; } = string.Empty;
    }
}