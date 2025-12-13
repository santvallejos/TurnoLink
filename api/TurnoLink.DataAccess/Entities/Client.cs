using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurnoLink.DataAccess.Entities
{
    /// <summary>
    /// Represents a client booking services
    /// </summary>
    [Table("clients")]
    public class Client
    {
        /// <summary>
        /// ID of the client
        /// </summary>
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the client
        /// </summary>
        [Required]
        [MaxLength(200)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Surname of the client
        /// </summary>
        [Required]
        [MaxLength(200)]
        [Column("surname")]
        public string Surname { get; set; } = string.Empty;

        /// <summary>
        /// Email of the client
        /// </summary>
        [Required]
        [MaxLength(255)]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Phone number of the client
        /// </summary>
        [MaxLength(20)]
        [Column("phone_number")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        /// <summary>
        /// Reservas realizadas por este cliente
        /// </summary>
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}