// ============================================================================
// Archivo: PayrollProcessDetail.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Payroll/PayrollProcessDetail.cs
// Descripción: Detalle del proceso de nómina para cada empleado.
//   - Cumple ISO 27001 heredando de AuditableCompanyEntity
//   - Contiene totales de pago, impuestos, TSS y datos de transferencia
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Infrastructure.TempScaffold;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa el detalle del proceso de nómina aplicado a un empleado.
    /// </summary>
    public class PayrollProcessDetail : AuditableCompanyEntity
    {
        /// <summary>
        /// FK al proceso de nómina correspondiente.
        /// </summary>
        public long PayrollProcessRefRecID { get; set; }

        /// <summary>
        /// FK al empleado procesado.
        /// </summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// Total a pagar al empleado.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Total de impuestos aplicados.
        /// </summary>
        public decimal TotalTaxAmount { get; set; }

        /// <summary>
        /// Método de pago utilizado (transferencia, cheque, efectivo).
        /// </summary>
        public int PayMethod { get; set; }

        /// <summary>
        /// Número de cuenta bancaria utilizada.
        /// </summary>
        public string? BankAccount { get; set; }

        /// <summary>
        /// Nombre del banco asociado al pago.
        /// </summary>
        public string? BankName { get; set; }

        /// <summary>
        /// Documento de referencia del pago (cheque, recibo).
        /// </summary>
        public string? Document { get; set; }

        /// <summary>
        /// FK al departamento del empleado.
        /// </summary>
        public long? DepartmentRefRecID { get; set; }

        /// <summary>
        /// Nombre del departamento del empleado.
        /// </summary>
        public string? DepartmentName { get; set; }

        /// <summary>
        /// Estado del proceso de nómina para este registro.
        /// </summary>
        public int PayrollProcessStatus { get; set; }

        /// <summary>
        /// Nombre del empleado.
        /// </summary>
        public string? EmployeeName { get; set; }

        /// <summary>
        /// Fecha de inicio de labores del empleado.
        /// </summary>
        public DateTime StartWorkDate { get; set; }

        /// <summary>
        /// Total de aportes a la TSS.
        /// </summary>
        public decimal TotalTssAmount { get; set; }

        /// <summary>
        /// Total de aportes a la TSS más impuestos.
        /// </summary>
        public decimal TotalTssAndTaxAmount { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Departamento relacionado.
        /// </summary>
        public virtual Department? DepartmentRefRec { get; set; }

        /// <summary>
        /// Empleado al que corresponde el detalle.
        /// </summary>
        public virtual Employee EmployeeRefRec { get; set; } = null!;

        /// <summary>
        /// Proceso de nómina relacionado.
        /// </summary>
        public virtual PayrollsProcess PayrollProcessRefRec { get; set; } = null!;
    }
}
