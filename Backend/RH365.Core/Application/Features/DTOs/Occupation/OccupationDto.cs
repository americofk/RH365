// ============================================================================
// Archivo: OccupationDto.cs
// Capa: RH365.Core.Application
// Ruta: Application/Features/DTOs/Occupation/OccupationDto.cs
// Descripción: DTO de salida para Occupations (lectura) con auditoría.
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.Occupation
{
    /// <summary>DTO de lectura para ocupaciones.</summary>
    public sealed class OccupationDto
    {
        public long RecID { get; set; }
        public string OccupationCode { get; set; } = null!;
        public string? Description { get; set; }

        // Auditoría
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
