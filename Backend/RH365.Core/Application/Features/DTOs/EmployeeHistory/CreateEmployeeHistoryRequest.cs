// ============================================================================
// Archivo: CreateEmployeeHistoryRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeHistory/CreateEmployeeHistoryRequest.cs
// Descripción:
//   - DTO de creación para EmployeeHistory
//   - No incluye ID/RecID (se generan en BD)
//   - Auditoría la maneja el DbContext automáticamente
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeHistory
{
    /// <summary>
    /// Payload para crear un evento en el historial laboral.
    /// </summary>
    public class CreateEmployeeHistoryRequest
    {
        /// <summary>Código único del evento en el historial.</summary>
        public string EmployeeHistoryCode { get; set; } = null!;

        /// <summary>Tipo de evento (máximo 5 caracteres: PROM, SANC, CAMB, etc.).</summary>
        public string Type { get; set; } = null!;

        /// <summary>Descripción detallada del evento.</summary>
        public string Description { get; set; } = null!;

        /// <summary>Fecha en la que ocurrió el evento.</summary>
        public DateTime RegisterDate { get; set; }

        /// <summary>FK al empleado relacionado con el evento.</summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>Indica si este evento se reporta a la DGT.</summary>
        public bool IsUseDgt { get; set; }

        /// <summary>Observaciones adicionales.</summary>
        public string? Observations { get; set; }
    }
}