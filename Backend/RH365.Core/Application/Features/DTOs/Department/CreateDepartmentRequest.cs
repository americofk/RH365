// ============================================================================
// Archivo: CreateDepartmentRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/Department/CreateDepartmentRequest.cs
// Descripción: DTO de entrada para crear un departamento.
//   - Contiene solo campos editables por el usuario
//   - Auditoría ISO 27001 se asigna automáticamente
// ============================================================================
using System;
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.Department
{
    /// <summary>Payload para crear un nuevo departamento.</summary>
    public sealed class CreateDepartmentRequest
    {
        [Required, StringLength(20, MinimumLength = 2)]
        public string DepartmentCode { get; set; } = null!;

        [Required, StringLength(60, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        public int QtyWorkers { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(100)]
        public string? Description { get; set; }

        public bool DepartmentStatus { get; set; }

        [StringLength(500)]
        public string? Observations { get; set; }
    }
}