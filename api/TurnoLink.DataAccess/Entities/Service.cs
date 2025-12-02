using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurnoLink.DataAccess.Entities
{
    /// <summary>
    /// Represents a service offered by a professional
    /// </summary>
    [Table("services")]
    public class Service
    {
        private decimal _price;

        /// <summary>
        /// ID of the service
        /// </summary>
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// ID of the professional offering the service
        /// </summary>
        [Required]
        [Column("user_id")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Name of the service
        /// </summary>
        [Required]
        [MaxLength(200)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description of the service
        /// </summary>
        [MaxLength(1000)]
        [Column("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Duration of the service in minutes
        /// </summary>
        [Column("duration_minutes")]
        public int DurationMinutes { get; set; }

        /// <summary>
        /// Price of the service
        /// </summary>
        [Column("price", TypeName = "decimal(18,2)")]
        public decimal Price 
        { 
            get => _price;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Price), "Price cannot be negative.");
                _price = value;
            }
        }

        /// <summary>
        /// Indicates if the service is active
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
        /// Professional offering the service
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        /// <summary>
        /// Bookings associated with the service
        /// </summary>
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}