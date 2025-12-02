using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TurnoLink.Business.DTOs;
using TurnoLink.Business.Services.Interfaces;
using TurnoLink.DataAccess.Entities;
using TurnoLink.DataAccess.Repositories.Interfaces;

namespace TurnoLink.Business.Services;

/// <summary>
/// Servicio de autenticación con JWT
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            // Validar que el email no exista
            var existingUser = await _unitOfWork.Users.GetByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return ApiResponse<AuthResponseDto>.ErrorResult("El email ya está registrado");
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

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            // Generar token
            var token = GenerateJwtToken(user);
            var expiresAt = DateTime.UtcNow.AddMinutes(GetTokenExpirationMinutes());

            var response = new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                FullName = user.FullName,
                UserId = user.Id,
                ExpiresAt = expiresAt
            };

            return ApiResponse<AuthResponseDto>.SuccessResult(response, "Usuario registrado exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<AuthResponseDto>.ErrorResult($"Error al registrar usuario: {ex.Message}");
        }
    }

    public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto)
    {
        try
        {
            // Buscar usuario por email
            var user = await _unitOfWork.Users.GetByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return ApiResponse<AuthResponseDto>.ErrorResult("Credenciales inválidas");
            }

            // Verificar que el usuario esté activo
            if (!user.IsActive)
            {
                return ApiResponse<AuthResponseDto>.ErrorResult("Usuario inactivo");
            }

            // Verificar contraseña
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return ApiResponse<AuthResponseDto>.ErrorResult("Credenciales inválidas");
            }

            // Generar token
            var token = GenerateJwtToken(user);
            var expiresAt = DateTime.UtcNow.AddMinutes(GetTokenExpirationMinutes());

            var response = new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                FullName = user.FullName,
                UserId = user.Id,
                ExpiresAt = expiresAt
            };

            return ApiResponse<AuthResponseDto>.SuccessResult(response, "Login exitoso");
        }
        catch (Exception ex)
        {
            return ApiResponse<AuthResponseDto>.ErrorResult($"Error al iniciar sesión: {ex.Message}");
        }
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
