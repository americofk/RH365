// ============================================================================
// Archivo: PayrollProcessAction.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Payroll/PayrollProcessAction.cs
// Descripción: Entidad que representa una acción aplicada a un empleado durante
//              el procesamiento de la nómina (bonos, descuentos, ajustes).
//   - Cumple ISO 27001 heredando de AuditableCompanyEntity
//   - Define tipo de acción, monto, y reglas de aplicación de impuestos/TSS
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Infrastructure.TempScaffold;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa una acción aplicada a un empleado durante el proceso de nómina.
    /// </summary>
    public class PayrollProcessAction : AuditableCompanyEntity
    {
        /// <summary>
        /// FK al proceso de nómina donde se ejecuta la acción.
        /// </summary>
        public long PayrollProcessRefRecID { get; set; }

        /// <summary>
        /// FK al empleado al que se aplica la acción.
        /// </summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// Tipo de acción (ej. bono, deducción, ajuste).
        /// </summary>
        public int PayrollActionType { get; set; }

        /// <summary>
        /// Nombre de la acción aplicada.
        /// </summary>
        public string ActionName { get; set; } = null!;

        /// <summary>
        /// Monto monetario de la acción.
        /// </summary>
        public decimal ActionAmount { get; set; }

        /// <summary>
        /// Indica si la acción aplica impuestos.
        /// </summary>
        public bool ApplyTax { get; set; }

        /// <summary>
        /// Indica si la acción aplica aportes a la TSS.
        /// </summary>
        public bool ApplyTss { get; set; }

        /// <summary>
        /// Indica si la acción aplica a la nómina de regalía.
        /// </summary>
        public bool ApplyRoyaltyPayroll { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Empleado al que se aplica la acción.
        /// </summary>
        public virtual Employee EmployeeRefRec { get; set; } = null!;

        /// <summary>
        /// Proceso de nómina al que pertenece la acción.
        /// </summary>
        public virtual PayrollsProcess PayrollProcessRefRec { get; set; } = null!;
    }
}
