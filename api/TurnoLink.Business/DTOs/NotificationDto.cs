namespace TurnoLink.Business.DTOs;

/// <summary>
/// DTO para lectura de notificación
/// </summary>
public class NotificationDto
{
    public Guid Id { get; set; }
    public Guid? BookingId { get; set; }
    public string NotificationType { get; set; } = string.Empty;
    public string Recipient { get; set; } = string.Empty;
    public string? Subject { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? SentAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO para crear una nueva notificación
/// </summary>
public class CreateNotificationDto
{
    public Guid? BookingId { get; set; }
    public string NotificationType { get; set; } = string.Empty;
    public string Recipient { get; set; } = string.Empty;
    public string? Subject { get; set; }
    public string Message { get; set; } = string.Empty;
}
