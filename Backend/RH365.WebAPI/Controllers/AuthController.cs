// ============================================================================
// Archivo: AuthController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/AuthController.cs
// Descripción: Controlador de autenticación y autorización.
//   - Login con JWT
//   - Validación de credenciales contra tabla Users
//   - Generación de tokens con claims multiempresa
// ============================================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RH365.WebAPI.Controllers
{
    /// <summary>
    /// Controlador para autenticación y autorización del sistema.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Constructor del controlador de autenticación.
        /// </summary>
        public AuthController(
            IApplicationDbContext context,
            IConfiguration configuration,
            ILogger<AuthController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Autentica un usuario y devuelve un token JWT.
        /// </summary>
        /// <param name="request">Credenciales del usuario.</param>
        /// <returns>Token JWT y información del usuario.</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Buscar usuario por email o alias
                var user = await _context.Users
                    .Include(u => u.CompaniesAssignedToUsers)
                        .ThenInclude(c => c.CompanyRefRec)
                    .Include(u => u.CompanyDefaultRefRec)
                    .Where(u => u.IsActive &&
                              (u.Email.ToLower() == request.EmailOrAlias.ToLower() ||
                               u.Alias.ToLower() == request.EmailOrAlias.ToLower()))
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    _logger.LogWarning("Intento de login fallido: usuario no encontrado {Email}", request.EmailOrAlias);
                    return Unauthorized("Credenciales inválidas");
                }

                // Verificar contraseña
                if (!VerifyPassword(request.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Intento de login fallido: contraseña incorrecta para {Email}", request.EmailOrAlias);
                    return Unauthorized("Credenciales inválidas");
                }

                // Verificar si necesita cambiar contraseña temporal
                bool requiresPasswordChange = !string.IsNullOrEmpty(user.TemporaryPassword) &&
                    user.DateTemporaryPassword.HasValue &&
                    user.DateTemporaryPassword.Value.AddDays(30) > DateTime.UtcNow;

                // Obtener empresas autorizadas
                var authorizedCompanies = user.CompaniesAssignedToUsers
                    .Where(c => c.IsActive)
                    .Select(c => new CompanyInfo
                    {
                        Id = c.CompanyRefRec.RecID.ToString(),
                        Code = c.CompanyRefRec.CompanyCode,
                        Name = c.CompanyRefRec.Name
                    })
                    .ToList();

                // Determinar empresa por defecto
                var defaultCompany = authorizedCompanies.FirstOrDefault(c =>
                    c.Id == user.CompanyDefaultRefRecID?.ToString()) ??
                    authorizedCompanies.FirstOrDefault();

                if (defaultCompany == null)
                {
                    return Unauthorized("Usuario no tiene empresas asignadas");
                }

                // Generar token JWT
                var token = GenerateJwtToken(user, authorizedCompanies, defaultCompany);

                // Log login exitoso
                _logger.LogInformation("Login exitoso para usuario {UserId} - {Email}", user.RecID, user.Email);

                var response = new LoginResponse
                {
                    Token = token,
                    TokenType = "Bearer",
                    ExpiresIn = int.Parse(_configuration["Jwt:ExpirationInMinutes"] ?? "480") * 60,
                    User = new UserInfo
                    {
                        Id = user.RecID,
                        Alias = user.Alias,
                        Name = user.Name,
                        Email = user.Email,
                        DefaultCompany = defaultCompany,
                        AuthorizedCompanies = authorizedCompanies,
                        RequiresPasswordChange = requiresPasswordChange
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el proceso de login");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Cambia la contraseña de un usuario autenticado.
        /// </summary>
        /// <param name="request">Datos para cambio de contraseña.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPost("change-password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!long.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized("Token inválido");
                }

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return Unauthorized("Usuario no encontrado");
                }

                // Verificar contraseña actual
                if (!VerifyPassword(request.CurrentPassword, user.PasswordHash))
                {
                    return BadRequest("Contraseña actual incorrecta");
                }

                // Actualizar contraseña
                user.PasswordHash = HashPassword(request.NewPassword);
                user.TemporaryPassword = null;
                user.DateTemporaryPassword = null;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Contraseña cambiada exitosamente para usuario {UserId}", userId);

                return Ok(new { Message = "Contraseña actualizada exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar contraseña");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene información del usuario actual autenticado.
        /// </summary>
        /// <returns>Información del usuario.</returns>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserInfo>> GetCurrentUser()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!long.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized("Token inválido");
                }

                var user = await _context.Users
                    .Include(u => u.CompaniesAssignedToUsers)
                        .ThenInclude(c => c.CompanyRefRec)
                    .Include(u => u.CompanyDefaultRefRec)
                    .Where(u => u.RecID == userId && u.IsActive)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return Unauthorized("Usuario no encontrado o inactivo");
                }

                var authorizedCompanies = user.CompaniesAssignedToUsers
                    .Where(c => c.IsActive)
                    .Select(c => new CompanyInfo
                    {
                        Id = c.CompanyRefRec.RecID.ToString(),
                        Code = c.CompanyRefRec.CompanyCode,
                        Name = c.CompanyRefRec.Name
                    })
                    .ToList();

                var defaultCompany = authorizedCompanies.FirstOrDefault(c =>
                    c.Id == user.CompanyDefaultRefRecID?.ToString()) ??
                    authorizedCompanies.FirstOrDefault();

                var userInfo = new UserInfo
                {
                    Id = user.RecID,
                    Alias = user.Alias,
                    Name = user.Name,
                    Email = user.Email,
                    DefaultCompany = defaultCompany,
                    AuthorizedCompanies = authorizedCompanies,
                    RequiresPasswordChange = !string.IsNullOrEmpty(user.TemporaryPassword)
                };

                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener información del usuario actual");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        #region Métodos privados

        /// <summary>
        /// Genera un token JWT para el usuario autenticado.
        /// </summary>
        private string GenerateJwtToken(User user, List<CompanyInfo> companies, CompanyInfo defaultCompany)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.RecID.ToString()),
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Email, user.Email),
                new("alias", user.Alias),
                new("companyId", defaultCompany.Code),
                new("companyName", defaultCompany.Name),
                new("companies", string.Join(",", companies.Select(c => c.Code)))
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpirationInMinutes"] ?? "480")),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Hashea una contraseña usando BCrypt.
        /// </summary>
        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Verifica una contraseña contra su hash.
        /// </summary>
        private static bool VerifyPassword(string password, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }

    #region DTOs

    /// <summary>
    /// DTO para petición de login.
    /// </summary>
    public class LoginRequest
    {
        [Required]
        [StringLength(255)]
        public string EmailOrAlias { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = null!;
    }

    /// <summary>
    /// DTO para respuesta de login exitoso.
    /// </summary>
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public string TokenType { get; set; } = null!;
        public int ExpiresIn { get; set; }
        public UserInfo User { get; set; } = null!;
    }

    /// <summary>
    /// DTO para cambio de contraseña.
    /// </summary>
    public class ChangePasswordRequest
    {
        [Required]
        public string CurrentPassword { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]",
            ErrorMessage = "La contraseña debe contener al menos: 1 minúscula, 1 mayúscula, 1 número y 1 carácter especial")]
        public string NewPassword { get; set; } = null!;

        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; } = null!;
    }

    /// <summary>
    /// DTO para información del usuario.
    /// </summary>
    public class UserInfo
    {
        public long Id { get; set; }
        public string Alias { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public CompanyInfo? DefaultCompany { get; set; }
        public List<CompanyInfo> AuthorizedCompanies { get; set; } = new();
        public bool RequiresPasswordChange { get; set; }
    }

    /// <summary>
    /// DTO para información de empresa.
    /// </summary>
    public class CompanyInfo
    {
        public string Id { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
    }

    #endregion
}