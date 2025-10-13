// ============================================================================
// Archivo: CreateEmployeePositionRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeePosition/CreateEmployeePositionRequest.cs
// Descripción:
//   - DTO de creación para EmployeePosition
//   - No incluye ID/RecID (se generan en BD); auditoría la maneja el DbContext
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeePosition
{
    /// <summary>
    /// Payload para crear una asignación de posición.
    /// </summary>
    public class CreateEmployeePositionRequest
    {
        public long EmployeeRefRecID { get; set; }
        public long PositionRefRecID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool EmployeePositionStatus { get; set; } = true;
        public string? Comment { get; set; }
    }
}