// ============================================================================
// Archivo: CreateGeneralConfigRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/GeneralConfig/CreateGeneralConfigRequest.cs
// Descripción:
//   - DTO de creación para GeneralConfig
//   - No incluye ID/RecID (se generan en BD); auditoría la maneja el DbContext
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.GeneralConfig
{
    /// <summary>
    /// Payload para crear una configuración general.
    /// </summary>
    public class CreateGeneralConfigRequest
    {
        public string Email { get; set; } = null!;
        public string Smtp { get; set; } = null!;
        public string Smtpport { get; set; } = null!;
        public string EmailPassword { get; set; } = null!;
        public string? Observations { get; set; }
    }
}