namespace TurnoLink.Business.DTOs;

/// <summary>
/// DTO para crear un nuevo servicio
/// </summary>
public class CreateServiceDto
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }
}

/// <summary>
/// DTO para actualizar un servicio existente
/// </summary>
public class UpdateServiceDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? DurationMinutes { get; set; }
    public decimal? Price { get; set; }
    public bool? IsActive { get; set; }
}

/// <summary>
/// DTO para respuesta de servicio
/// </summary>
public class ServiceDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
