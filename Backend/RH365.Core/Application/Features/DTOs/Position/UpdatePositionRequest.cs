// ============================================================================
// Archivo: UpdatePositionRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/DTOs/Position/UpdatePositionRequest.cs
// Descripción: DTO para actualización de puestos de trabajo.
// ============================================================================
using System;
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.DTOs.Position
{
    public class UpdatePositionRequest
    {
        [Required, StringLength(10)]
        public string PositionCode { get; set; } = null!;

        [Required, StringLength(255)]
        public string PositionName { get; set; } = null!;

        public bool IsVacant { get; set; }

        [Required]
        public long DepartmentRefRecID { get; set; }

        [Required]
        public long JobRefRecID { get; set; }

        public long? NotifyPositionRefRecID { get; set; }

        public bool PositionStatus { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
