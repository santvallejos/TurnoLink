using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurnoLink.DataAccess.Entities;

/// <summary>
/// Representa la disponibilidad horaria de un profesional
/// </summary>
[Table("availabilities")]
public class Availability
{
    /// <summary>
    /// Identificador único de la disponibilidad
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// ID del profesional
    /// </summary>
    [Required]
    [Column("user_id")]
    public Guid UserId { get; set; }

    /// <summary>
    /// Día de la semana (0=Domingo, 6=Sábado)
    /// </summary>
    [Column("day_of_week")]
    public int DayOfWeek { get; set; }

    /// <summary>
    /// Hora de inicio de disponibilidad
    /// </summary>
    [Required]
    [Column("start_time")]
    public TimeSpan StartTime { get; set; }

    /// <summary>
    /// Hora de fin de disponibilidad
    /// </summary>
    [Required]
    [Column("end_time")]
    public TimeSpan EndTime { get; set; }

    /// <summary>
    /// Indica si esta disponibilidad está activa
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
    /// Profesional asociado a esta disponibilidad
    /// </summary>
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}
