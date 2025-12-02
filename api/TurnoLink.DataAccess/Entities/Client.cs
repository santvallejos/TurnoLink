using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurnoLink.DataAccess.Entities;

/// <summary>
/// Representa un cliente que reserva servicios
/// </summary>
[Table("clients")]
public class Client
{
    /// <summary>
    /// Identificador único del cliente
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre completo del cliente
    /// </summary>
    [Required]
    [MaxLength(200)]
    [Column("full_name")]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del cliente
    /// </summary>
    [Required]
    [MaxLength(255)]
    [Column("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Número de teléfono
    /// </summary>
    [MaxLength(20)]
    [Column("phone_number")]
    public string? PhoneNumber { get; set; }

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
    /// Reservas realizadas por este cliente
    /// </summary>
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
