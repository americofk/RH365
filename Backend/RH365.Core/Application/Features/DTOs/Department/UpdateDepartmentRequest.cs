// ============================================================================
// Archivo: UpdateDepartmentRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/Department/UpdateDepartmentRequest.cs
// Descripción: DTO de entrada para actualizar un departamento existente.
//   - Todos los campos son opcionales (nullable)
//   - Solo se actualizan los campos enviados
// ============================================================================
using System;
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.Department
{
    /// <summary>Payload para actualizar un departamento existente.</summary>
    public sealed class UpdateDepartmentRequest
    {
        [StringLength(20, MinimumLength = 2)]
        public string? DepartmentCode { get; set; }

        [StringLength(60, MinimumLength = 2)]
        public string? Name { get; set; }

        public int? QtyWorkers { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(100)]
        public string? Description { get; set; }

        public bool? DepartmentStatus { get; set; }

        [StringLength(500)]
        public string? Observations { get; set; }
    }
}