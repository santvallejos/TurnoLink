using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TurnoLink.DataAccess.Enums;

namespace TurnoLink.DataAccess.Entities
{
    /// <summary>
    /// Represents the availability of a professional (user) for bookings.
    /// All DateTime fields are stored in UTC format.
    /// </summary>
    [Table("availabilities")]
    public class Availability
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Required]
        [Column("service_id")]
        public Guid ServiceId { get; set; }

        [Required]
        [Column("start_time_utc")]
        public DateTime StartTimeUtc { get; set; }

        [Required]
        [Column("end_time_utc")]
        public DateTime EndTimeUtc { get; set; }

        [Required]
        [Column("repeat")]
        public RepeatAvailability Repeat { get; set; } = RepeatAvailability.None;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}