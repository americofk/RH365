// ============================================================================
// Archivo: DepartmentDto.cs
// Proyecto: RH365.Core
// Ruta: Application/Features/DTOs/Department/DepartmentDto.cs
// Descripción: DTO de lectura para Departamentos (usa RecID como PK real).
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.Department
{
    /// <summary>DTO para respuestas de departamentos.</summary>
    public sealed class DepartmentDto
    {
        public long RecID { get; set; }
        public string DepartmentCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
