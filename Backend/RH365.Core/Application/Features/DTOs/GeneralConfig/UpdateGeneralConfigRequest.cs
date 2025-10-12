// ============================================================================
// Archivo: UpdateGeneralConfigRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/GeneralConfig/UpdateGeneralConfigRequest.cs
// Descripción:
//   - DTO de actualización parcial para GeneralConfig
//   - Todos los campos son opcionales; solo se actualiza lo enviado
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.GeneralConfig
{
    /// <summary>
    /// Payload para actualizar (parcial) una configuración general.
    /// </summary>
    public class UpdateGeneralConfigRequest
    {
        public string? Email { get; set; }
        public string? Smtp { get; set; }
        public string? Smtpport { get; set; }
        public string? EmailPassword { get; set; }
        public string? Observations { get; set; }
    }
}