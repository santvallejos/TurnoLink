using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TurnoLink.Business.DTOs;
using TurnoLink.Business.Interfaces;
using TurnoLink.DataAccess.Data;
using TurnoLink.DataAccess.Entities;
using TurnoLink.DataAccess.Interfaces;

namespace TurnoLink.Business.Services
{
    /// <summary>
    /// Servicio de autenticación con JWT
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly TurnoLinkDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, TurnoLinkDbContext context, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // Validar que el email no exista
            var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("El email ya está registrado");
            }

            // Hash de contraseña
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // Crear usuario
            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                PhoneNumber = registerDto.PhoneNumber,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            await _context.SaveChangesAsync();

            // Generar token
            var token = GenerateJwtToken(user);
            var expiresAt = DateTime.UtcNow.AddMinutes(GetTokenExpirationMinutes());

            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                FullName = user.FullName,
                UserId = user.Id,
                ExpiresAt = expiresAt
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            // Buscar usuario por email
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Credenciales inválidas");
            }

            // Verificar que el usuario esté activo
            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("Usuario inactivo");
            }

            // Verificar contraseña
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Credenciales inválidas");
            }

            // Generar token
            var token = GenerateJwtToken(user);
            var expiresAt = DateTime.UtcNow.AddMinutes(GetTokenExpirationMinutes());

            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                FullName = user.FullName,
                UserId = user.Id,
                ExpiresAt = expiresAt
            };
        }

        public Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(GetSecretKey());

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = GetIssuer(),
                    ValidateAudience = true,
                    ValidAudience = GetAudience(),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(GetSecretKey());

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("userId", user.Id.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(GetTokenExpirationMinutes()),
                Issuer = GetIssuer(),
                Audience = GetAudience(),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GetSecretKey()
        {
            return _configuration["Jwt:SecretKey"] 
                ?? throw new InvalidOperationException("JWT SecretKey no configurada");
        }

        private string GetIssuer()
        {
            return _configuration["Jwt:Issuer"] ?? "TurnoLink";
        }

        private string GetAudience()
        {
            return _configuration["Jwt:Audience"] ?? "TurnoLinkUsers";
        }

        private int GetTokenExpirationMinutes()
        {
            return int.TryParse(_configuration["Jwt:ExpirationMinutes"], out var minutes) 
                ? minutes 
                : 60;
        }
    }
}
