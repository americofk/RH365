// ============================================================================
// Archivo: EducationLevelDto.cs
// Capa: RH365.Core.Application
// Ruta: Application/Features/DTOs/EducationLevel/EducationLevelDto.cs
// Descripción: DTO de salida para EducationLevels (lectura) con auditoría.
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EducationLevel
{
    /// <summary>DTO de lectura para niveles educativos.</summary>
    public sealed class EducationLevelDto
    {
        public long RecID { get; set; }
        public string EducationLevelCode { get; set; } = null!;
        public string? Description { get; set; }

        // Auditoría
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
