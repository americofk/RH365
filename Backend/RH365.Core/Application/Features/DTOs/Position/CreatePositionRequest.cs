// ============================================================================
// Archivo: CreatePositionRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/Position/CreatePositionRequest.cs
// Descripción: DTO para creación de puestos de trabajo.
//   - Contiene solo campos editables por el usuario
//   - Auditoría ISO 27001 se asigna automáticamente
// ============================================================================
using System;
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.DTOs.Position
{
    public class CreatePositionRequest
    {
        [Required, StringLength(20)]
        public string PositionCode { get; set; } = null!;

        [Required, StringLength(50)]
        public string PositionName { get; set; } = null!;

        public bool IsVacant { get; set; } = true;

        [Required]
        public long DepartmentRefRecID { get; set; }

        [Required]
        public long JobRefRecID { get; set; }

        public long? NotifyPositionRefRecID { get; set; }

        public bool PositionStatus { get; set; } = true;

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        [StringLength(500)]
        public string? Observations { get; set; }
    }
}