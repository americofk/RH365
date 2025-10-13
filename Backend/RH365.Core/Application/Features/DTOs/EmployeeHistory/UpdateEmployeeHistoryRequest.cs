// ============================================================================
// Archivo: UpdateEmployeeHistoryRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeHistory/UpdateEmployeeHistoryRequest.cs
// Descripción:
//   - DTO de actualización parcial para EmployeeHistory
//   - Todos los campos son opcionales
//   - Solo se actualiza lo enviado en el request
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeHistory
{
    /// <summary>
    /// Payload para actualizar (parcial) un evento del historial laboral.
    /// </summary>
    public class UpdateEmployeeHistoryRequest
    {
        /// <summary>Código único del evento en el historial.</summary>
        public string? EmployeeHistoryCode { get; set; }

        /// <summary>Tipo de evento (máximo 5 caracteres).</summary>
        public string? Type { get; set; }

        /// <summary>Descripción detallada del evento.</summary>
        public string? Description { get; set; }

        /// <summary>Fecha en la que ocurrió el evento.</summary>
        public DateTime? RegisterDate { get; set; }

        /// <summary>FK al empleado relacionado con el evento.</summary>
        public long? EmployeeRefRecID { get; set; }

        /// <summary>Indica si este evento se reporta a la DGT.</summary>
        public bool? IsUseDgt { get; set; }

        /// <summary>Observaciones adicionales.</summary>
        public string? Observations { get; set; }
    }
}