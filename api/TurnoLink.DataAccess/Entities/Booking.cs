using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurnoLink.DataAccess.Entities;

/// <summary>
/// Representa una reserva/cita de un cliente
/// </summary>
[Table("bookings")]
public class Booking
{
    /// <summary>
    /// Identificador único de la reserva
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// ID del cliente que realiza la reserva
    /// </summary>
    [Required]
    [Column("client_id")]
    public Guid ClientId { get; set; }

    /// <summary>
    /// ID del servicio reservado
    /// </summary>
    [Required]
    [Column("service_id")]
    public Guid ServiceId { get; set; }

    /// <summary>
    /// ID del profesional que atenderá
    /// </summary>
    [Required]
    [Column("user_id")]
    public Guid UserId { get; set; }

    /// <summary>
    /// Fecha y hora de inicio de la reserva
    /// </summary>
    [Required]
    [Column("start_time")]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Fecha y hora de fin de la reserva
    /// </summary>
    [Required]
    [Column("end_time")]
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Estado de la reserva
    /// </summary>
    [Required]
    [MaxLength(50)]
    [Column("status")]
    public string Status { get; set; } = BookingStatus.Pending;

    /// <summary>
    /// Notas adicionales de la reserva
    /// </summary>
    [MaxLength(500)]
    [Column("notes")]
    public string? Notes { get; set; }

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
    /// Cliente que realizó la reserva
    /// </summary>
    [ForeignKey("ClientId")]
    public virtual Client Client { get; set; } = null!;

    /// <summary>
    /// Servicio reservado
    /// </summary>
    [ForeignKey("ServiceId")]
    public virtual Service Service { get; set; } = null!;

    /// <summary>
    /// Profesional que atenderá
    /// </summary>
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Notificaciones asociadas a esta reserva
    /// </summary>
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}

/// <summary>
/// Estados posibles de una reserva
/// </summary>
public static class BookingStatus
{
    public const string Pending = "Pending";
    public const string Confirmed = "Confirmed";
    public const string Completed = "Completed";
    public const string Cancelled = "Cancelled";
    public const string NoShow = "NoShow";
}
