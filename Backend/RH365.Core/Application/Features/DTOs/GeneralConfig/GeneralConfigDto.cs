// ============================================================================
// Archivo: GeneralConfigDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/GeneralConfig/GeneralConfigDto.cs
// Descripción:
//   - DTO de lectura para la entidad GeneralConfig
//   - EmailPassword enmascarado por seguridad
//   - Incluye claves, datos y auditoría (ISO 27001)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.GeneralConfig
{
    /// <summary>
    /// DTO de salida para GeneralConfig.
    /// </summary>
    public class GeneralConfigDto
    {
        public long RecID { get; set; }
        public string? ID { get; set; }
        public string Email { get; set; } = null!;
        public string Smtp { get; set; } = null!;
        public string Smtpport { get; set; } = null!;
        public string EmailPassword { get; set; } = null!;
        public string? Observations { get; set; }
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}