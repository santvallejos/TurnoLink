using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurnoLink.DataAccess.Entities
{
    /// <summary>
    /// Represents a user in the system
    /// </summary>
    [Table("users")]
    public class User
    {
        /// <summary>
        /// ID of the user
        /// </summary>
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Full name of the user
        /// </summary>
        [Required]
        [MaxLength(200)]
        [Column("full_name")]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Email of the user
        /// </summary>
        [Required]
        [MaxLength(255)]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Password hash
        /// </summary>
        [Required]
        [MaxLength(500)]
        [Column("password_hash")]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Phone number
        /// </summary>
        [MaxLength(20)]
        [Column("phone_number")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Indicates if the user is active
        /// </summary>
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Record creation date
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Last update date
        /// </summary>
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        /// <summary>
        /// Services offered by this professional
        /// </summary>
        public virtual ICollection<Service> Services { get; set; } = new List<Service>();

        /// <summary>
        /// Bookings associated with this professional
        /// </summary>
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}