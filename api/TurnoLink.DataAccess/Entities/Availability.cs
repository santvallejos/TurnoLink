using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurnoLink.DataAccess.Entities
{
    /// <summary>
    /// Represents the availability of a professional (user) for bookings.
    /// </summary>
    [Table("availabilities")]
    public class Availability
    {
        /// <summary>
        /// ID of the availability entry
        /// </summary>
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// ID of the professional (user)
        /// </summary>
        [Required]
        [Column("user_id")]
        public Guid UserId { get; set; }

        /// <sumary>
        /// ID of the service associated with this availability
        /// </sumary>
        [Required]
        [Column("service_id")]
        public Guid ServiceId { get; set; }

        /// <summary>
        /// Date and time when the availability starts
        /// </summary>
        [Required]
        [Column("start_time")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Duration of the availability in minutes
        /// </summary>
        [Column("duration_minutes")]
        public int DurationMinutes { get; set; }
    }
}