using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurnoLink.DataAccess.Entities;

/// <summary>
/// Representa una notificación enviada al cliente o profesional
/// </summary>
[Table("notifications")]
public class Notification
{
    /// <summary>
    /// Identificador único de la notificación
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// ID de la reserva asociada
    /// </summary>
    [Column("booking_id")]
    public Guid? BookingId { get; set; }

    /// <summary>
    /// Tipo de notificación
    /// </summary>
    [Required]
    [MaxLength(50)]
    [Column("notification_type")]
    public string NotificationType { get; set; } = NotificationTypes.Email;

    /// <summary>
    /// Destinatario (email, teléfono, etc.)
    /// </summary>
    [Required]
    [MaxLength(255)]
    [Column("recipient")]
    public string Recipient { get; set; } = string.Empty;

    /// <summary>
    /// Asunto o título de la notificación
    /// </summary>
    [MaxLength(200)]
    [Column("subject")]
    public string? Subject { get; set; }

    /// <summary>
    /// Contenido del mensaje
    /// </summary>
    [Required]
    [MaxLength(2000)]
    [Column("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Estado del envío
    /// </summary>
    [Required]
    [MaxLength(50)]
    [Column("status")]
    public string Status { get; set; } = NotificationStatus.Pending;

    /// <summary>
    /// Fecha de envío
    /// </summary>
    [Column("sent_at")]
    public DateTime? SentAt { get; set; }

    /// <summary>
    /// Fecha de creación del registro
    /// </summary>
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    /// <summary>
    /// Reserva asociada (opcional)
    /// </summary>
    [ForeignKey("BookingId")]
    public virtual Booking? Booking { get; set; }
}

/// <summary>
/// Tipos de notificación disponibles
/// </summary>
public static class NotificationTypes
{
    public const string Email = "Email";
    public const string WhatsApp = "WhatsApp";
    public const string Sms = "SMS";
}

/// <summary>
/// Estados de envío de notificaciones
/// </summary>
public static class NotificationStatus
{
    public const string Pending = "Pending";
    public const string Sent = "Sent";
    public const string Failed = "Failed";
}
