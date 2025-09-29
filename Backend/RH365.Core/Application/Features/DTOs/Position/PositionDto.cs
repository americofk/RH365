// ============================================================================
// Archivo: PositionDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/DTOs/Position/PositionDto.cs
// Descripción: DTO para representar un puesto de trabajo en respuestas API.
// ============================================================================
using System;

namespace RH365.Core.Application.DTOs.Position
{
    /// <summary>
    /// DTO para exponer información de un puesto de trabajo.
    /// </summary>
    public class PositionDto
    {
        public long RecID { get; set; }
        public string PositionCode { get; set; } = null!;
        public string PositionName { get; set; } = null!;
        public bool IsVacant { get; set; }
        public long DepartmentRefRecID { get; set; }
        public long JobRefRecID { get; set; }
        public long? NotifyPositionRefRecID { get; set; }
        public bool PositionStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
