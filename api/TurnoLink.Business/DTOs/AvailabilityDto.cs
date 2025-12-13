using System.ComponentModel.DataAnnotations;
using TurnoLink.DataAccess.Enums;

namespace TurnoLink.Business.DTOs
{
    public class CreateAvailabilityDto
    {
        
        [Required(ErrorMessage = "Service ID is required")]
        public Guid ServiceId { get; set; }

        [Required(ErrorMessage = "Start date and time is required")]
        public DateTime StartTime { get; set; }
    }

    public class CreateRecurringAvailabilityDto : CreateAvailabilityDto
    {
        private RepeatAvailability _repeat;

        [Required(ErrorMessage = "End date and time is required")]
        public RepeatAvailability Repeat 
        { 
            get => _repeat; 
            set
            {
                if (value == RepeatAvailability.None)
                {
                    throw new ValidationException("Repeat frequency must be specified for recurring availability.");
                }
                else
                {
                    _repeat = value;
                }
            }
        }

        public DateTime EndTime { get; set;}
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