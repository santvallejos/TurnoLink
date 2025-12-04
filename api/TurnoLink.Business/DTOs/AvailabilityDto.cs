using System.ComponentModel.DataAnnotations;

namespace TurnoLink.Business.DTOs;

/// <summary>
/// DTO para crear una nueva disponibilidad (slot de tiempo específico)
/// </summary>
public class CreateAvailabilityDto
{
    /// <summary>
    /// ID del servicio asociado a esta disponibilidad
    /// </summary>
    [Required(ErrorMessage = "El ID del servicio es requerido")]
    public Guid ServiceId { get; set; }

    /// <summary>
    /// Fecha y hora de inicio del slot de disponibilidad
    /// </summary>
    [Required(ErrorMessage = "La fecha y hora de inicio es requerida")]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Duración del slot en minutos
    /// </summary>
    [Required(ErrorMessage = "La duración es requerida")]
    [Range(15, 480, ErrorMessage = "La duración debe estar entre 15 minutos y 8 horas")]
    public int DurationMinutes { get; set; }
}

/// <summary>
/// DTO para actualizar una disponibilidad existente
/// </summary>
public class UpdateAvailabilityDto
{
    /// <summary>
    /// Nueva fecha y hora de inicio
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// Nueva duración en minutos
    /// </summary>
    [Range(15, 480, ErrorMessage = "La duración debe estar entre 15 minutos y 8 horas")]
    public int? DurationMinutes { get; set; }
}

/// <summary>
/// DTO para respuesta de disponibilidad
/// </summary>
public class AvailabilityDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ServiceId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int DurationMinutes { get; set; }
    public string ServiceName { get; set; } = string.Empty;
}
