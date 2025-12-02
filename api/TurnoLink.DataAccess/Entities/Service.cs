using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurnoLink.DataAccess.Entities;

/// <summary>
/// Representa un servicio ofrecido por un profesional
/// </summary>
[Table("services")]
public class Service
{
    /// <summary>
    /// Identificador único del servicio
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// ID del profesional que ofrece el servicio
    /// </summary>
    [Required]
    [Column("user_id")]
    public Guid UserId { get; set; }

    /// <summary>
    /// Nombre del servicio
    /// </summary>
    [Required]
    [MaxLength(200)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Descripción del servicio
    /// </summary>
    [MaxLength(1000)]
    [Column("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Duración del servicio en minutos
    /// </summary>
    [Column("duration_minutes")]
    public int DurationMinutes { get; set; }

    /// <summary>
    /// Precio del servicio
    /// </summary>
    [Column("price", TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    /// <summary>
    /// Indica si el servicio está activo
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
    /// Profesional que ofrece este servicio
    /// </summary>
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Reservas de este servicio
    /// </summary>
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
