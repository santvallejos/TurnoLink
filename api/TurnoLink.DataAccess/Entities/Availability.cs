using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TurnoLink.DataAccess.Enums;

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

        /// <summary>
        /// ID of the service associated with this availability
        /// </summary>
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
        /// Date and time when the availability ends
        /// </summary>
        [Column("end_time")]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Repeat availability
        /// </summary>
        [Required]
        [Column("repeat")]
        public RepeatAvailability Repeat { get; set; } = RepeatAvailability.None;

        /// Navegation properties
        /// <summary>
        /// Service being booked
        /// </summary>
        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; } = null!;

        /// <summary>
        /// Professional (user) providing the service
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}