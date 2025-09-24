// ============================================================================
// Archivo: EmployeeTax.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Employees/EmployeeTax.cs
// Descripción: Relación que representa los impuestos aplicados a un empleado.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Permite registrar impuestos asociados a la nómina de un empleado
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Infrastructure.TempScaffold;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un impuesto aplicado a un empleado dentro de la nómina.
    /// </summary>
    public class EmployeeTax : AuditableCompanyEntity
    {
        /// <summary>
        /// FK al impuesto aplicado.
        /// </summary>
        public long TaxRefRecID { get; set; }

        /// <summary>
        /// FK al empleado afectado por el impuesto.
        /// </summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// Fecha de inicio de vigencia del impuesto.
        /// </summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// Fecha de finalización de vigencia del impuesto.
        /// </summary>
        public DateTime ValidTo { get; set; }

        /// <summary>
        /// FK a la nómina en la que se aplica el impuesto.
        /// </summary>
        public long PayrollRefRecID { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Empleado relacionado con el impuesto.
        /// </summary>
        public virtual Employee EmployeeRefRec { get; set; } = null!;

        /// <summary>
        /// Nómina relacionada con el impuesto.
        /// </summary>
        public virtual Payroll PayrollRefRec { get; set; } = null!;

        /// <summary>
        /// Impuesto aplicado al empleado.
        /// </summary>
        public virtual Taxis TaxRefRec { get; set; } = null!;
    }
}
