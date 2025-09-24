// ============================================================================
// Archivo: PayCycle.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Payroll/PayCycle.cs
// Descripción: Entidad que representa los ciclos de pago de una nómina.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Permite definir períodos de pago, montos y fechas asociadas
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Infrastructure.TempScaffold;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un ciclo de pago dentro de una nómina.
    /// </summary>
    public class PayCycle : AuditableCompanyEntity
    {
        /// <summary>
        /// Identificador interno del ciclo de pago.
        /// </summary>
        public int PayCycleId { get; set; }

        /// <summary>
        /// FK a la nómina a la que pertenece este ciclo.
        /// </summary>
        public long PayrollRefRecID { get; set; }

        /// <summary>
        /// Fecha de inicio del período de pago.
        /// </summary>
        public DateTime PeriodStartDate { get; set; }

        /// <summary>
        /// Fecha de finalización del período de pago.
        /// </summary>
        public DateTime PeriodEndDate { get; set; }

        /// <summary>
        /// Fecha de pago predeterminada para el ciclo.
        /// </summary>
        public DateTime DefaultPayDate { get; set; }

        /// <summary>
        /// Fecha real en que se realiza el pago.
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// Monto total pagado en el período.
        /// </summary>
        public decimal AmountPaidPerPeriod { get; set; }

        /// <summary>
        /// Estado del ciclo de pago.
        /// </summary>
        public int StatusPeriod { get; set; }

        /// <summary>
        /// Indica si el ciclo es considerado para impuestos.
        /// </summary>
        public bool IsForTax { get; set; }

        /// <summary>
        /// Indica si el ciclo es considerado para la TSS.
        /// </summary>
        public bool IsForTss { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Nómina relacionada con el ciclo de pago.
        /// </summary>
        public virtual Payroll PayrollRefRec { get; set; } = null!;
    }
}
