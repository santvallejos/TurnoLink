namespace TurnoLink.Business.DTOs;

/// <summary>
/// DTO para crear un nuevo cliente
/// </summary>
public class CreateClientDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
}

/// <summary>
/// DTO para actualizar un cliente existente
/// </summary>
public class UpdateClientDto
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}

/// <summary>
/// DTO para respuesta de cliente
/// </summary>
public class ClientDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; }
}
