// ============================================================================
// Archivo: ChangePasswordRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Models/Auth/ChangePasswordRequest.cs
// Descripción:
//   - Modelo para solicitud de cambio de contraseña
//   - Requiere contraseña actual y nueva
// ============================================================================

namespace RH365.Core.Domain.Models.Auth
{
    /// <summary>
    /// Request para cambio de contraseña
    /// </summary>
    public class ChangePasswordRequest
    {
        /// <summary>
        /// Contraseña actual del usuario
        /// </summary>
        public string CurrentPassword { get; set; } = null!;

        /// <summary>
        /// Nueva contraseña
        /// </summary>
        public string NewPassword { get; set; } = null!;

        /// <summary>
        /// Confirmación de nueva contraseña
        /// </summary>
        public string ConfirmPassword { get; set; } = null!;
    }
}