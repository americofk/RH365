// ============================================================================
// Archivo: LoginCommandHandler.cs
// Proyecto: D365_API_Nomina.Infrastructure
// Ruta: D365_API_Nomina.Infrastructure/Application/Handlers/Login/LoginCommandHandler.cs
// Descripción: Implementación mínima de ILoginCommandHandler que autentica
//              contra la tabla Users utilizando los campos reales del modelo:
//              Alias, Email, PasswordHash, CompanyDefaultRefRecID, etc.
//              *Esta versión no consulta Company ni asignaciones; devuelve
//              la lista de compañías vacía hasta mapear esas entidades.*
// ============================================================================

using System;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using D365_API_Nomina.Infrastructure.Persistence.Configuration;      // ApplicationDbContext
using D365_API_Nomina.Core.Application.Contracts.Login;              // LoginRequest/LoginResponse
using D365_API_Nomina.Core.Application.Handlers.Login;               // ILoginCommandHandler
using D365_API_Nomina.Core.Domain.Entities;                          // User

namespace D365_API_Nomina.Infrastructure.Application.Handlers.Login
{
    /// <summary>
    /// Handler de Login (versión mínima sin compañías).
    /// </summary>
    public class LoginCommandHandler : ILoginCommandHandler
    {
        private readonly ApplicationDbContext _db;
        private readonly string _jwtSecret;

        public LoginCommandHandler(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _jwtSecret = configuration["Jwt:Secret"]
                         ?? throw new InvalidOperationException("Config 'Jwt:Secret' no encontrada.");
        }

        public async Task<LoginResponse?> Login(LoginRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
                return null;

            // Nota: aquí se compara PasswordHash con el valor plano recibido.
            // Cuando tengamos el helper de hashing, reemplazamos por la verificación correspondiente.
            var user = await _db.Set<User>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    (x.Email == model.Email || x.Alias == model.Email) &&
                    (model.IsValidateUser || x.PasswordHash == (model.Password ?? string.Empty))
                );

            if (user == null)
                return null;

            // Generar JWT con alias, email y "compañía por defecto" como string (temporal).
            string defaultCompany = (user.CompanyDefaultRefRecID?.ToString()) ?? string.Empty;
            string token = GenerateJwtToken(user.Alias, user.Email, defaultCompany);

            return new LoginResponse
            {
                Token = token,
                Name = user.Name,
                Alias = user.Alias,
                Avatar = string.Empty,               // pendiente mapear imagen
                FormatCode = user.FormatCodeRefRecID.ToString(),
                DefaultCompany = defaultCompany,
                Email = user.Email,
                UserCompanies = new()                       // vacío hasta mapear Company/Asignaciones
            };
        }

        private string GenerateJwtToken(string alias, string email, string defaultCompany)
        {
            var keyBytes = Encoding.ASCII.GetBytes(_jwtSecret);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, alias),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.PostalCode, defaultCompany) // compañía por defecto (temporal)
            };

            var creds = new SigningCredentials(new SymmetricSecurityKey(keyBytes),
                                               SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
