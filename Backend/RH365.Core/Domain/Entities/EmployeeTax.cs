// ============================================================================
// Archivo: EmployeeTax.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Employees/EmployeeTax.cs
// Descripción: Entidad que representa los impuestos aplicados a un empleado.
//              Hereda de AuditableCompanyEntity (ISO 27001: RecID, ID,
//              CreatedBy/On, ModifiedBy/On, DataareaID, Observations, RowVersion).
// Notas:
//  - FKs obligatorias (*RefRecID): TaxRefRecID, EmployeeRefRecID, PayrollRefRecID.
//  - Navegaciones: EmployeeRefRec, PayrollRefRec, TaxRefRec.
//  - El mapeo de columnas/FKs/índices se define en
//    RH365.Infrastructure/Persistence/Configurations/Employees/EmployeeTaxConfiguration.cs
// ============================================================================

using System;
using RH365.Core.Domain.Common;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un impuesto aplicado a un empleado dentro del sistema de nómina.
    /// </summary>
    public class EmployeeTax : AuditableCompanyEntity
    {
        /// <summary>FK al impuesto aplicado (dbo.Taxes.RecID).</summary>
        public long TaxRefRecID { get; set; }

        /// <summary>FK al empleado afectado (dbo.Employees.RecID).</summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>Fecha de inicio de vigencia del impuesto.</summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>Fecha de finalización de vigencia del impuesto.</summary>
        public DateTime ValidTo { get; set; }

        /// <summary>FK a la nómina donde se aplica (dbo.Payrolls.RecID).</summary>
        public long PayrollRefRecID { get; set; }

        // ---------------------------------------------------------------------
        // Propiedades de navegación (configuradas en la Configuration)
        // ---------------------------------------------------------------------

        /// <summary>Empleado relacionado con el impuesto.</summary>
        public virtual Employee EmployeeRefRec { get; set; } = null!;

        /// <summary>Nómina relacionada con el impuesto.</summary>
        public virtual Payroll PayrollRefRec { get; set; } = null!;

        /// <summary>Impuesto aplicado (entidad Taxis que mapea a dbo.Taxes).</summary>
        public virtual Taxis TaxRefRec { get; set; } = null!;
    }
}