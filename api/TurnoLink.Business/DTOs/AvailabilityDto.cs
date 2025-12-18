using System.ComponentModel.DataAnnotations;
using TurnoLink.DataAccess.Enums;

namespace TurnoLink.Business.DTOs
{
    public class CreateAvailabilityDto
    {
        [Required(ErrorMessage = "Service ID is required")]
        public Guid ServiceId { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        public TimeOnly StartTime { get; set; }


        [Required(ErrorMessage = "UTC offset is required for timezone conversion")]
        public int UtcOffsetMinutes { get; set; }
    }

    public class CreateRecurringAvailabilityDto
    {
        [Required(ErrorMessage = "Service ID is required")]
        public Guid ServiceId { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        public TimeOnly StartTime { get; set; }

        [Required(ErrorMessage = "UTC offset is required for timezone conversion")]
        public int UtcOffsetMinutes { get; set; }

        [Required(ErrorMessage = "Repeat frequency is required")]
        public RepeatAvailability Repeat { get; set; }

        [Required(ErrorMessage = "End date is required for recurring availability")]
        public DateOnly EndDate { get; set; }
    }

    public class UpdateAvailabilityDto
    {
        public DateOnly? NewDate { get; set; }

        public TimeOnly? NewTime { get; set; }

        public int? UtcOffsetMinutes { get; set; }
    }

    public class AvailabilityDto
    {
        public Guid Id { get; set; }

        public Guid ServiceId { get; set; }

        public DateTime StartTimeUtc { get; set; }

        public DateTime EndTimeUtc { get; set; }

        public string ServiceName { get; set; } = string.Empty;

        public int DurationMinutes { get; set; }

        public RepeatAvailability Repeat { get; set; }

        public bool IsBooked { get; set; }
    }
}