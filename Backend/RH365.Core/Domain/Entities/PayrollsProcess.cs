// ============================================================================
// Archivo: PayrollsProcess.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Payroll/PayrollsProcess.cs
// Descripción: Entidad que representa un proceso de nómina ejecutado en el sistema.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Contiene información de períodos, pagos, estado y relación con empleados
// ============================================================================

using RH365.Core.Domain.Common;
using System;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un proceso de nómina ejecutado en el sistema.
    /// </summary>
    public class PayrollsProcess : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único del proceso de nómina.
        /// </summary>
        public string PayrollProcessCode { get; set; } = null!;

        /// <summary>
        /// FK a la nómina asociada al proceso.
        /// </summary>
        public long? PayrollRefRecID { get; set; }

        /// <summary>
        /// Descripción del proceso de nómina.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Fecha en la que se realiza el pago.
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// Cantidad de empleados incluidos en el proceso.
        /// </summary>
        public int EmployeeQuantity { get; set; }

        /// <summary>
        /// Proyecto asociado (opcional).
        /// </summary>
        public string? ProjId { get; set; }

        /// <summary>
        /// Categoría del proyecto asociada (opcional).
        /// </summary>
        public string? ProjCategoryId { get; set; }

        /// <summary>
        /// Fecha de inicio del período de nómina.
        /// </summary>
        public DateTime PeriodStartDate { get; set; }

        /// <summary>
        /// Fecha de fin del período de nómina.
        /// </summary>
        public DateTime PeriodEndDate { get; set; }

        /// <summary>
        /// Identificador del ciclo de pago.
        /// </summary>
        public int PayCycleId { get; set; }

        /// <summary>
        /// Cantidad de empleados incluidos para pago.
        /// </summary>
        public int EmployeeQuantityForPay { get; set; }

        /// <summary>
        /// Estado del proceso de nómina.
        /// </summary>
        public int PayrollProcessStatus { get; set; }

        /// <summary>
        /// Indica si el ciclo aplica impuestos.
        /// </summary>
        public bool IsPayCycleTax { get; set; }

        /// <summary>
        /// Indica si el proceso fue usado para impuestos.
        /// </summary>
        public bool UsedForTax { get; set; }

        /// <summary>
        /// Indica si aplica a la nómina de regalía.
        /// </summary>
        public bool IsRoyaltyPayroll { get; set; }

        /// <summary>
        /// Indica si el ciclo aplica TSS.
        /// </summary>
        public bool IsPayCycleTss { get; set; }

        /// <summary>
        /// Indica si el proceso fue usado para TSS.
        /// </summary>
        public bool UsedForTss { get; set; }

        /// <summary>
        /// Indica si aplica a nómina por horas.
        /// </summary>
        public bool IsForHourPayroll { get; set; }

        /// <summary>
        /// Monto total a pagar en el proceso.
        /// </summary>
        public decimal TotalAmountToPay { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Códigos de percepciones asociados.
        /// </summary>
        public virtual ICollection<EmployeeEarningCode> EmployeeEarningCodes { get; set; } = new List<EmployeeEarningCode>();

        /// <summary>
        /// Historial de préstamos de empleados incluidos en el proceso.
        /// </summary>
        public virtual ICollection<EmployeeLoanHistory> EmployeeLoanHistories { get; set; } = new List<EmployeeLoanHistory>();

        /// <summary>
        /// Acciones aplicadas a los empleados en el proceso.
        /// </summary>
        public virtual ICollection<PayrollProcessAction> PayrollProcessActions { get; set; } = new List<PayrollProcessAction>();

        /// <summary>
        /// Detalles individuales por empleado en el proceso.
        /// </summary>
        public virtual ICollection<PayrollProcessDetail> PayrollProcessDetails { get; set; } = new List<PayrollProcessDetail>();

        /// <summary>
        /// Nómina relacionada con el proceso.
        /// </summary>
        public virtual Payroll? PayrollRefRec { get; set; }
    }
}
