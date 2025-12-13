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
        /// <summary>
        /// ID of the booking
        /// </summary>
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// ID of the client who made the booking
        /// </summary>
        [Required]
        [Column("client_id")]
        public Guid ClientId { get; set; }

        /// <summary>
        /// ID of the service being booked
        /// </summary>
        [Required]
        [Column("service_id")]
        public Guid ServiceId { get; set; }

        /// <summary>
        /// ID of the availability slot for the booking
        /// </summary>
        [Required]
        [Column("availability_id")]
        public Guid AvailabilityId { get; set; }

        /// <summary>
        /// ID of the professional (user) who will provide the service
        /// </summary>
        [Required]
        [Column("user_id")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Date and time when the booking starts
        /// </summary>
        [Required]
        [Column("start_time")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Date and time when the booking ends
        /// </summary>
        [Required]
        [Column("end_time")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Status of the booking
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Column("status")]
        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        /// <summary>
        /// Notes or additional information about the booking
        /// </summary>
        [MaxLength(500)]
        [Column("notes")]
        public string? Notes { get; set; }

        /// <summary>
        /// Date and time when the record was created
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        /// <summary>
        /// Client who made the booking
        /// </summary>
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; } = null!;

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

        /// <summary>
        /// Availability slot for the booking
        /// </summary>
        [ForeignKey("AvailabilityId")]
        public virtual Availability Availability { get; set; } = null!;
    }
}