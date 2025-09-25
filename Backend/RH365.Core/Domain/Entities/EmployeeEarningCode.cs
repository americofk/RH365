// ============================================================================
// Archivo: EmployeeEarningCode.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Employees/EmployeeEarningCode.cs
// Descripción: Relación que representa las percepciones asignadas a un empleado.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Permite registrar valores fijos, horas extras, periodicidad y reglas de cálculo
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Core.Domain.Entities;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa una percepción (earning) asignada a un empleado dentro de la nómina.
    /// </summary>
    public class EmployeeEarningCode : AuditableCompanyEntity
    {
        /// <summary>
        /// FK al código de percepción.
        /// </summary>
        public long EarningCodeRefRecID { get; set; }

        /// <summary>
        /// FK al empleado que recibe la percepción.
        /// </summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// FK a la nómina donde se aplica la percepción.
        /// </summary>
        public long PayrollRefRecID { get; set; }

        /// <summary>
        /// FK al proceso de nómina (si aplica).
        /// </summary>
        public long? PayrollProcessRefRecID { get; set; }

        /// <summary>
        /// Fecha de inicio de aplicación.
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Fecha de finalización de aplicación.
        /// </summary>
        public DateTime ToDate { get; set; }

        /// <summary>
        /// Índice de cálculo de la percepción.
        /// </summary>
        public decimal IndexEarning { get; set; }

        /// <summary>
        /// Cantidad de unidades (ejemplo: horas).
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Comentario adicional sobre la percepción.
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Cantidad de períodos de pago.
        /// </summary>
        public int QtyPeriodForPaid { get; set; }

        /// <summary>
        /// Período inicial de pago.
        /// </summary>
        public int StartPeriodForPaid { get; set; }

        /// <summary>
        /// Índice de percepción mensual.
        /// </summary>
        public decimal IndexEarningMonthly { get; set; }

        /// <summary>
        /// Frecuencia de pago.
        /// </summary>
        public int PayFrecuency { get; set; }

        /// <summary>
        /// Índice de percepción diaria.
        /// </summary>
        public decimal IndexEarningDiary { get; set; }

        /// <summary>
        /// Indica si se usa para reportes a la DGT.
        /// </summary>
        public bool IsUseDgt { get; set; }

        /// <summary>
        /// Índice de percepción por hora.
        /// </summary>
        public decimal IndexEarningHour { get; set; }

        /// <summary>
        /// Indica si se utiliza cálculo por hora.
        /// </summary>
        public bool IsUseCalcHour { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Código de percepción relacionado.
        /// </summary>
        public virtual EarningCode EarningCodeRefRec { get; set; } = null!;

        /// <summary>
        /// Empleado relacionado.
        /// </summary>
        public virtual Employee EmployeeRefRec { get; set; } = null!;

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
