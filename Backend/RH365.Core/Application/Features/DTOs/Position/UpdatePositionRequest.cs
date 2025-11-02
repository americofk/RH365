// ============================================================================
// Archivo: UpdatePositionRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/Position/UpdatePositionRequest.cs
// Descripción: DTO para actualización de puestos de trabajo.
//   - Todos los campos son opcionales (nullable)
//   - Solo se actualizan los campos enviados
// ============================================================================
using System;
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.DTOs.Position
{
    public class UpdatePositionRequest
    {
        [StringLength(20)]
        public string? PositionCode { get; set; }

        [StringLength(50)]
        public string? PositionName { get; set; }

        public bool? IsVacant { get; set; }

        public long? DepartmentRefRecID { get; set; }

        public long? JobRefRecID { get; set; }

        public long? NotifyPositionRefRecID { get; set; }

        public bool? PositionStatus { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        [StringLength(500)]
        public string? Observations { get; set; }
    }
}