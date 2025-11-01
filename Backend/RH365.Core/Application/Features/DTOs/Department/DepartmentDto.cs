// ============================================================================
// Archivo: DepartmentDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/Department/DepartmentDto.cs
// Descripción: DTO de lectura para Departamentos.
//   - Incluye todos los campos de la tabla
//   - Auditoría ISO 27001 completa
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.Department
{
    /// <summary>DTO para respuestas de departamentos.</summary>
    public sealed class DepartmentDto
    {
        public long RecID { get; set; }
        public string? ID { get; set; }
        public string DepartmentCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int QtyWorkers { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }
        public bool DepartmentStatus { get; set; }
        public string? Observations { get; set; }
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}