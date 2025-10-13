// ============================================================================
// Archivo: LoginRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Models/Auth/LoginRequest.cs
// Descripción:
//   - Modelo para solicitud de login desde el frontend MVC
//   - Se mapea al DTO del API backend
// ============================================================================

namespace RH365.Core.Domain.Models.Auth
{
    /// <summary>
    /// Request para autenticación de usuarios
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Email o alias del usuario
        /// </summary>
        public string EmailOrAlias { get; set; } = null!;

        /// <summary>
        /// Contraseña del usuario
        /// </summary>
        public string Password { get; set; } = null!;

        /// <summary>
        /// ID de empresa (opcional para multiempresa)
        /// </summary>
        public string? CompanyId { get; set; }
    }
}