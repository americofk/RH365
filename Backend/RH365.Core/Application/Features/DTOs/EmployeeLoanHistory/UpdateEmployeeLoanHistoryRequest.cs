// ============================================================================
// Archivo: UpdateEmployeeLoanHistoryRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeLoanHistory/UpdateEmployeeLoanHistoryRequest.cs
// Descripción:
//   - DTO de actualización parcial para EmployeeLoanHistory
//   - Todos los campos son opcionales
//   - Solo se actualiza lo enviado en el request
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeLoanHistory
{
    /// <summary>
    /// Payload para actualizar (parcial) un registro de historial de préstamo.
    /// </summary>
    public class UpdateEmployeeLoanHistoryRequest
    {
        /// <summary>FK al préstamo del empleado.</summary>
        public long? EmployeeLoanRefRecID { get; set; }

        /// <summary>FK al préstamo relacionado.</summary>
        public long? LoanRefRecID { get; set; }

        /// <summary>FK al empleado beneficiario.</summary>
        public long? EmployeeRefRecID { get; set; }

        /// <summary>Fecha de inicio del período de pago.</summary>
        public DateTime? PeriodStartDate { get; set; }

        /// <summary>Fecha de finalización del período de pago.</summary>
        public DateTime? PeriodEndDate { get; set; }

        /// <summary>FK a la nómina en la que se descuenta el pago.</summary>
        public long? PayrollRefRecID { get; set; }

        /// <summary>FK al proceso de nómina (opcional).</summary>
        public long? PayrollProcessRefRecID { get; set; }

        /// <summary>Monto total del préstamo en este período.</summary>
        public decimal? LoanAmount { get; set; }

        /// <summary>Monto pagado en este período.</summary>
        public decimal? PaidAmount { get; set; }

        /// <summary>Monto pendiente en este período.</summary>
        public decimal? PendingAmount { get; set; }

        /// <summary>Cantidad total de cuotas en este período.</summary>
        public int? TotalDues { get; set; }

        /// <summary>Cantidad de cuotas pendientes en este período.</summary>
        public int? PendingDues { get; set; }

        /// <summary>Monto por cada cuota.</summary>
        public decimal? AmountByDues { get; set; }

        /// <summary>Observaciones adicionales.</summary>
        public string? Observations { get; set; }
    }
}