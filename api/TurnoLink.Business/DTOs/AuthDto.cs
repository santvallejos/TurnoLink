using System.ComponentModel.DataAnnotations;

namespace TurnoLink.Business.DTOs
{
    public class LoginDto
    {
        [Required (ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;
        [Required (ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterDto
    {
        [Required (ErrorMessage = "Full name is required")]
        public string FullName { get; set; } = string.Empty;
        [Required (ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;
        [Required (ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }

    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}