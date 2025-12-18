using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TurnoLink.DataAccess.Enums;

namespace TurnoLink.DataAccess.Entities
{
    /// <summary>
    /// Represents a booking made by a client for a service with a professional.
    /// </summary>
    [Table("bookings")]
    public class Booking
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("client_id")]
        public Guid ClientId { get; set; }

        [Required]
        [Column("service_id")]
        public Guid ServiceId { get; set; }

        [Required]
        [Column("availability_id")]
        public Guid AvailabilityId { get; set; }

        [Required]
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Required]
        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Required]
        [Column("end_time")]
        public DateTime EndTime { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("status")]
        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        [MaxLength(500)]
        [Column("notes")]
        public string? Notes { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; } = null!;

        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("AvailabilityId")]
        public virtual Availability Availability { get; set; } = null!;
    }
}