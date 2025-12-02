using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurnoLink.DataAccess.Entities;

/// <summary>
/// Representa un usuario profesional del sistema que ofrece servicios
/// </summary>
[Table("users")]
public class User
{
    /// <summary>
    /// Identificador único del usuario
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre completo del profesional
    /// </summary>
    [Required]
    [MaxLength(200)]
    [Column("full_name")]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del usuario
    /// </summary>
    [Required]
    [MaxLength(255)]
    [Column("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hash de la contraseña
    /// </summary>
    [Required]
    [MaxLength(500)]
    [Column("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Número de teléfono
    /// </summary>
    [MaxLength(20)]
    [Column("phone_number")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Indica si el usuario está activo
    /// </summary>
    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Fecha de creación del registro
    /// </summary>
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha de última actualización
    /// </summary>
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    /// <summary>
    /// Servicios ofrecidos por este profesional
    /// </summary>
    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    /// <summary>
    /// Disponibilidades configuradas
    /// </summary>
    public virtual ICollection<Availability> Availabilities { get; set; } = new List<Availability>();

    /// <summary>
    /// Reservas asociadas a este profesional
    /// </summary>
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
