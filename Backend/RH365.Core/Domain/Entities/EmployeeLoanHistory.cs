// ============================================================================
// Archivo: EmployeeLoanHistory.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Employees/EmployeeLoanHistory.cs
// Descripción: Entidad que representa el historial de pagos de préstamos de empleados.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Permite registrar montos, períodos, cuotas pagadas y pendientes
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Infrastructure.TempScaffold;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un registro en el historial de pagos de préstamos de un empleado.
    /// </summary>
    public class EmployeeLoanHistory : AuditableCompanyEntity
    {
        /// <summary>
        /// FK al préstamo del empleado.
        /// </summary>
        public long EmployeeLoanRefRecID { get; set; }

        /// <summary>
        /// FK al préstamo relacionado.
        /// </summary>
        public long LoanRefRecID { get; set; }

        /// <summary>
        /// FK al empleado beneficiario.
        /// </summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// Fecha de inicio del período de pago.
        /// </summary>
        public DateTime PeriodStartDate { get; set; }

        /// <summary>
        /// Fecha de finalización del período de pago.
        /// </summary>
        public DateTime PeriodEndDate { get; set; }

        /// <summary>
        /// FK a la nómina en la que se descuenta el pago.
        /// </summary>
        public long PayrollRefRecID { get; set; }

        /// <summary>
        /// FK al proceso de nómina (si aplica).
        /// </summary>
        public long? PayrollProcessRefRecID { get; set; }

        /// <summary>
        /// Monto total del préstamo en este período.
        /// </summary>
        public decimal LoanAmount { get; set; }

        /// <summary>
        /// Monto pagado en este período.
        /// </summary>
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// Monto pendiente en este período.
        /// </summary>
        public decimal PendingAmount { get; set; }

        /// <summary>
        /// Cantidad total de cuotas en este período.
        /// </summary>
        public int TotalDues { get; set; }

        /// <summary>
        /// Cantidad de cuotas pendientes en este período.
        /// </summary>
        public int PendingDues { get; set; }

        /// <summary>
        /// Monto por cada cuota.
        /// </summary>
        public decimal AmountByDues { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Relación con el préstamo del empleado.
        /// </summary>
        public virtual EmployeeLoan EmployeeLoanRefRec { get; set; } = null!;

        /// <summary>
        /// Empleado beneficiario.
        /// </summary>
        public virtual Employee EmployeeRefRec { get; set; } = null!;

        /// <summary>
        /// Préstamo relacionado.
        /// </summary>
        public virtual Loan LoanRefRec { get; set; } = null!;

        /// <summary>
        /// Proceso de nómina relacionado.
        /// </summary>
        public virtual PayrollsProcess? PayrollProcessRefRec { get; set; }

        /// <summary>
        /// Nómina relacionada.
        /// </summary>
        public virtual Payroll PayrollRefRec { get; set; } = null!;
    }
}
