// ============================================================================
// Archivo: EmployeeLoan.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Employees/EmployeeLoan.cs
// Descripción: Relación que representa los préstamos otorgados a empleados.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Permite registrar montos, cuotas, pagos y saldos pendientes
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Core.Domain.Entities;
using System;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un préstamo asignado a un empleado.
    /// </summary>
    public class EmployeeLoan : AuditableCompanyEntity
    {
        /// <summary>
        /// FK al préstamo otorgado.
        /// </summary>
        public long LoanRefRecID { get; set; }

        /// <summary>
        /// FK al empleado beneficiario del préstamo.
        /// </summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// Fecha de inicio de vigencia del préstamo.
        /// </summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// Fecha de finalización de vigencia del préstamo.
        /// </summary>
        public DateTime ValidTo { get; set; }

        /// <summary>
        /// Monto total del préstamo.
        /// </summary>
        public decimal LoanAmount { get; set; }

        /// <summary>
        /// Período en que inicia el pago.
        /// </summary>
        public int StartPeriodForPaid { get; set; }

        /// <summary>
        /// Monto ya pagado del préstamo.
        /// </summary>
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// Monto pendiente de pago del préstamo.
        /// </summary>
        public decimal PendingAmount { get; set; }

        /// <summary>
        /// FK a la nómina en la que se descuenta el préstamo.
        /// </summary>
        public long PayrollRefRecID { get; set; }

        /// <summary>
        /// Cantidad total de cuotas definidas.
        /// </summary>
        public int TotalDues { get; set; }

        /// <summary>
        /// Cantidad de cuotas pendientes de pago.
        /// </summary>
        public int PendingDues { get; set; }

        /// <summary>
        /// Cantidad de períodos de pago definidos.
        /// </summary>
        public int QtyPeriodForPaid { get; set; }

        /// <summary>
        /// Monto asignado por cada cuota.
        /// </summary>
        public decimal AmountByDues { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Historial de pagos del préstamo.
        /// </summary>
        public virtual ICollection<EmployeeLoanHistory> EmployeeLoanHistories { get; set; } = new List<EmployeeLoanHistory>();

        /// <summary>
        /// Empleado beneficiario del préstamo.
        /// </summary>
        public virtual Employee EmployeeRefRec { get; set; } = null!;

        /// <summary>
        /// Préstamo relacionado.
        /// </summary>
        public virtual Loan LoanRefRec { get; set; } = null!;

        /// <summary>
        /// Nómina relacionada con los descuentos.
        /// </summary>
        public virtual Payroll PayrollRefRec { get; set; } = null!;
    }
}
