// ============================================================================
// Archivo: TaxDetail.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Payroll/TaxDetail.cs
// Descripción: Entidad que representa los detalles de cálculo de un impuesto.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Define escalas, porcentajes y montos aplicables a un impuesto
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Infrastructure.TempScaffold;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa los detalles de cálculo de un impuesto.
    /// </summary>
    public class TaxDetail : AuditableCompanyEntity
    {
        /// <summary>
        /// FK al impuesto al que pertenece este detalle.
        /// </summary>
        public long TaxRefRecID { get; set; }

        /// <summary>
        /// Monto anual mayor a partir del cual aplica la escala.
        /// </summary>
        public decimal AnnualAmountHigher { get; set; }

        /// <summary>
        /// Monto anual máximo que no debe ser excedido en la escala.
        /// </summary>
        public decimal AnnualAmountNotExceed { get; set; }

        /// <summary>
        /// Porcentaje aplicable de impuesto.
        /// </summary>
        public decimal Percent { get; set; }

        /// <summary>
        /// Monto fijo a aplicar en la escala.
        /// </summary>
        public decimal FixedAmount { get; set; }

        /// <summary>
        /// Escala aplicable al cálculo.
        /// </summary>
        public decimal ApplicableScale { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Impuesto al que pertenece el detalle.
        /// </summary>
        public virtual Taxis TaxRefRec { get; set; } = null!;
    }
}
