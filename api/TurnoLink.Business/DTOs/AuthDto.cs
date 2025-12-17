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
        [Required (ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Surname is required")]
        public string Surname { get; set; } = string.Empty;
        
        [Required (ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;
        
        [Required (ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        [Required (ErrorMessage = "Repeat password is required")]
        public string RepeatPassword { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Address where the professional offers their services
        /// </summary>
        public string? Address { get; set; }
    }

    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}