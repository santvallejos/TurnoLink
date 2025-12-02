namespace TurnoLink.Business.DTOs;

/// <summary>
/// DTO para crear una nueva disponibilidad
/// </summary>
public class CreateAvailabilityDto
{
    public Guid UserId { get; set; }
    public int DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}

/// <summary>
/// DTO para actualizar una disponibilidad existente
/// </summary>
public class UpdateAvailabilityDto
{
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public bool? IsActive { get; set; }
}

/// <summary>
/// DTO para respuesta de disponibilidad
/// </summary>
public class AvailabilityDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public int DayOfWeek { get; set; }
    public string DayOfWeekName { get; set; } = string.Empty;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
