// ============================================================================
// Archivo: EmployeePositionDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeePosition/EmployeePositionDto.cs
// Descripción:
//   - DTO de lectura para la entidad EmployeePosition
//   - Incluye claves, datos y auditoría (ISO 27001)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeePosition
{
    /// <summary>
    /// DTO de salida para EmployeePosition.
    /// </summary>
    public class EmployeePositionDto
    {
        public long RecID { get; set; }
        public string? ID { get; set; }
        public long EmployeeRefRecID { get; set; }
        public long PositionRefRecID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool EmployeePositionStatus { get; set; }
        public string? Comment { get; set; }
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}