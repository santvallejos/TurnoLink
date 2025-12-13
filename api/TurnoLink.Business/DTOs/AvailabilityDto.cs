using System.ComponentModel.DataAnnotations;
using TurnoLink.DataAccess.Enums;

namespace TurnoLink.Business.DTOs
{
    public class CreateAvailabilityDto
    {
        private DateTime _endTime;
        [Required(ErrorMessage = "Service ID is required")]
        public Guid ServiceId { get; set; }

        [Required(ErrorMessage = "Start date and time is required")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End date and time is required")]
        public RepeatAvailability Repeat 
        { get; set; }

        public DateTime? EndTime 
        { 
            get => _endTime; 
            set
            {
                if (Repeat != RepeatAvailability.None && value == null)
                {
                    throw new ValidationException("EndTime is required when Repeat is set");
                }
                _endTime = value ?? DateTime.MinValue;
            }
        }
    }

    public class UpdateAvailabilityDto
    {
        public DateTime? StartTime { get; set; }
    }

    public class AvailabilityDto
    {
        public Guid Id { get; set; }
        public Guid ServiceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ServiceName { get; set; } = string.Empty;
    }
}