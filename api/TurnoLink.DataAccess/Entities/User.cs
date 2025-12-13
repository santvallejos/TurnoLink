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
        /// Name of the user
        /// </summary>
        [Required]
        [MaxLength(200)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Surname of the user
        /// </summary>
        [Required]
        [MaxLength(200)]
        [Column("surname")]
        public string Surname { get; set; } = string.Empty;

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
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Slug for the user profile
        /// </summary>
        [Required]
        [MaxLength(100)]
        [Column("slug")]
        public string Slug { get; set; } = string.Empty;

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

        // Navigation properties
        /// <summary>
        /// Services offered by this professional
        /// </summary>
        public virtual ICollection<Service> Services { get; set; } = new List<Service>();

        /// <summary>
        /// Bookings associated with this professional
        /// </summary>
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        /// <summary>
        /// Availabilities created by this professional
        /// </summary>
        public virtual ICollection<Availability> Availabilities { get; set; } = new List<Availability>();
    }
}