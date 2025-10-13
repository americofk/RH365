// ============================================================================
// Archivo: EmployeeLoanHistoryDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeLoanHistory/EmployeeLoanHistoryDto.cs
// Descripción:
//   - DTO de lectura para la entidad EmployeeLoanHistory
//   - Incluye claves, datos y auditoría (ISO 27001)
//   - Usado para respuestas GET
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeLoanHistory
{
    /// <summary>
    /// DTO de salida para EmployeeLoanHistory.
    /// </summary>
    public class EmployeeLoanHistoryDto
    {
        /// <summary>Clave primaria del registro.</summary>
        public long RecID { get; set; }

        /// <summary>Identificador legible generado por secuencia.</summary>
        public string? ID { get; set; }

        /// <summary>FK al préstamo del empleado.</summary>
        public long EmployeeLoanRefRecID { get; set; }

        /// <summary>FK al préstamo relacionado.</summary>
        public long LoanRefRecID { get; set; }

        /// <summary>FK al empleado beneficiario.</summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>Fecha de inicio del período de pago.</summary>
        public DateTime PeriodStartDate { get; set; }

        /// <summary>Fecha de finalización del período de pago.</summary>
        public DateTime PeriodEndDate { get; set; }

        /// <summary>FK a la nómina en la que se descuenta el pago.</summary>
        public long PayrollRefRecID { get; set; }

        /// <summary>FK al proceso de nómina (opcional).</summary>
        public long? PayrollProcessRefRecID { get; set; }

        /// <summary>Monto total del préstamo en este período.</summary>
        public decimal LoanAmount { get; set; }

        /// <summary>Monto pagado en este período.</summary>
        public decimal PaidAmount { get; set; }

        /// <summary>Monto pendiente en este período.</summary>
        public decimal PendingAmount { get; set; }

        /// <summary>Cantidad total de cuotas en este período.</summary>
        public int TotalDues { get; set; }

        /// <summary>Cantidad de cuotas pendientes en este período.</summary>
        public int PendingDues { get; set; }

        /// <summary>Monto por cada cuota.</summary>
        public decimal AmountByDues { get; set; }

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