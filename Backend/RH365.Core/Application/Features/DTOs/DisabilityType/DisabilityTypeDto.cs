// ============================================================================
// Archivo: DisabilityTypeDto.cs
// Capa: RH365.Core.Application
// Ruta: Application/Features/DTOs/DisabilityType/DisabilityTypeDto.cs
// Descripción: DTO de salida para DisabilityTypes (lectura).
//   - Incluye campos de auditoría de ISO 27001.
// ============================================================================

using System;

namespace RH365.Core.Application.Features.DTOs.DisabilityType
{
    /// <summary>DTO de lectura para tipos de discapacidad.</summary>
    public sealed class DisabilityTypeDto
    {
        public long RecID { get; set; }
        public string DisabilityTypeCode { get; set; } = null!;
        public string? Description { get; set; }

        // Auditoría
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
