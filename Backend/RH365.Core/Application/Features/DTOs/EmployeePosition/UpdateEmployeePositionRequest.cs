// ============================================================================
// Archivo: UpdateEmployeePositionRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeePosition/UpdateEmployeePositionRequest.cs
// Descripción:
//   - DTO de actualización parcial para EmployeePosition
//   - Todos los campos son opcionales; solo se actualiza lo enviado
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeePosition
{
    /// <summary>
    /// Payload para actualizar (parcial) una asignación de posición.
    /// </summary>
    public class UpdateEmployeePositionRequest
    {
        public long? EmployeeRefRecID { get; set; }
        public long? PositionRefRecID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? EmployeePositionStatus { get; set; }
        public string? Comment { get; set; }
    }
}