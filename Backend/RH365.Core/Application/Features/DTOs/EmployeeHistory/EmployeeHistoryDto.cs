// ============================================================================
// Archivo: EmployeeHistoryDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeHistory/EmployeeHistoryDto.cs
// Descripción:
//   - DTO de lectura para la entidad EmployeeHistory
//   - Incluye claves, datos y auditoría (ISO 27001)
//   - Usado para respuestas GET
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeHistory
{
    /// <summary>
    /// DTO de salida para EmployeeHistory.
    /// </summary>
    public class EmployeeHistoryDto
    {
        /// <summary>Clave primaria del registro.</summary>
        public long RecID { get; set; }

        /// <summary>Identificador legible generado por secuencia.</summary>
        public string? ID { get; set; }

        /// <summary>Código único del evento en el historial.</summary>
        public string EmployeeHistoryCode { get; set; } = null!;

        /// <summary>Tipo de evento (PROM, SANC, CAMB, etc.).</summary>
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

        // Auditoría ISO 27001
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}