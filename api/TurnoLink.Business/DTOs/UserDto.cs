namespace TurnoLink.Business.DTOs;

/// <summary>
/// DTO para crear un nuevo usuario
/// </summary>
public class CreateUserDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
}

/// <summary>
/// DTO para actualizar un usuario existente
/// </summary>
public class UpdateUserDto
{
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? IsActive { get; set; }
}

/// <summary>
/// DTO para respuesta de usuario
/// </summary>
public class UserDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
