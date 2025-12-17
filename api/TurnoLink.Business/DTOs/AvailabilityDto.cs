using System.ComponentModel.DataAnnotations;
using TurnoLink.DataAccess.Enums;

namespace TurnoLink.Business.DTOs
{
    /// <summary>
    /// DTO for creating a single availability slot using DayOfWeek and TimeSpan (timezone-safe)
    /// </summary>
    public class CreateAvailabilityDto
    {
        [Required(ErrorMessage = "Service ID is required")]
        public Guid ServiceId { get; set; }

        /// <summary>
        /// Day of the week (0 = Sunday, 1 = Monday, ..., 6 = Saturday)
        /// </summary>
        [Required(ErrorMessage = "Day of week is required")]
        [Range(0, 6, ErrorMessage = "Day of week must be between 0 (Sunday) and 6 (Saturday)")]
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// Start time of the availability (e.g., "09:00:00" for 9 AM)
        /// </summary>
        [Required(ErrorMessage = "Start time is required")]
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Optional: specific date to start creating availabilities from
        /// If not provided, will use the next occurrence of the specified DayOfWeek
        /// </summary>
        public DateTime? StartDate { get; set; }
    }

    /// <summary>
    /// DTO for creating recurring availability using DayOfWeek and TimeSpan (timezone-safe)
    /// </summary>
    public class CreateRecurringAvailabilityDto
    {
        [Required(ErrorMessage = "Service ID is required")]
        public Guid ServiceId { get; set; }

        /// <summary>
        /// Day of the week (0 = Sunday, 1 = Monday, ..., 6 = Saturday)
        /// </summary>
        [Required(ErrorMessage = "Day of week is required")]
        [Range(0, 6, ErrorMessage = "Day of week must be between 0 (Sunday) and 6 (Saturday)")]
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// Start time of the availability (e.g., "09:00:00" for 9 AM)
        /// </summary>
        [Required(ErrorMessage = "Start time is required")]
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// How often to repeat the availability
        /// </summary>
        [Required(ErrorMessage = "Repeat frequency is required")]
        public RepeatAvailability Repeat { get; set; }

        /// <summary>
        /// End date for recurring availabilities
        /// </summary>
        [Required(ErrorMessage = "End date is required for recurring availability")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Optional: specific date to start creating availabilities from
        /// If not provided, will use the next occurrence of the specified DayOfWeek
        /// </summary>
        public DateTime? StartDate { get; set; }
    }

    /// <summary>
    /// DTO for updating availability using DayOfWeek and TimeSpan
    /// </summary>
    public class UpdateAvailabilityDto
    {
        /// <summary>
        /// New day of the week (optional)
        /// </summary>
        public DayOfWeek? DayOfWeek { get; set; }

        /// <summary>
        /// New start time (optional)
        /// </summary>
        public TimeSpan? StartTime { get; set; }

        /// <summary>
        /// New specific date (optional)
        /// </summary>
        public DateTime? NewDate { get; set; }
    }

    public class AvailabilityDto
    {
        public Guid Id { get; set; }
        public Guid ServiceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        
        /// <summary>
        /// Day of the week for this availability
        /// </summary>
        public DayOfWeek DayOfWeek => StartTime.DayOfWeek;
        
        /// <summary>
        /// Time of day for this availability
        /// </summary>
        public TimeSpan TimeOfDay => StartTime.TimeOfDay;

        /// <summary>
        /// Indicates if this availability slot is already booked
        /// </summary>
        public bool IsBooked { get; set; }
    }
}