// ============================================================================
// Archivo: JobDto.cs
// Proyecto: RH365.Core
// Ruta: Application/Features/DTOs/Job/JobDto.cs
// Descripción: DTO de lectura para Jobs (PK = RecID).
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.Job
{
    /// <summary>DTO de lectura para puestos de trabajo (Jobs).</summary>
    public sealed class JobDto
    {
        public long RecID { get; set; }
        public string JobCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool JobStatus { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
