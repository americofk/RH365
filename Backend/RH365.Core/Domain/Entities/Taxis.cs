// ============================================================================
// Archivo: Taxis.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Payroll/Taxis.cs
// Descripción: Entidad que representa los impuestos configurados en el sistema.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Define reglas de cálculo, períodos, límites y relaciones con proyectos
// ============================================================================

using RH365.Core.Domain.Common;
using System;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un impuesto configurado en el sistema.
    /// </summary>
    public class Taxis : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único del impuesto.
        /// </summary>
        public string TaxCode { get; set; } = null!;

        /// <summary>
        /// Nombre del impuesto.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Cuenta contable asociada al impuesto.
        /// </summary>
        public string? LedgerAccount { get; set; }

        /// <summary>
        /// Fecha de inicio de vigencia del impuesto.
        /// </summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// Fecha de fin de vigencia del impuesto.
        /// </summary>
        public DateTime ValidTo { get; set; }

        /// <summary>
        /// FK a la moneda en la que se expresa el impuesto.
        /// </summary>
        public long CurrencyRefRecID { get; set; }

        /// <summary>
        /// Factor multiplicador para el cálculo del impuesto.
        /// </summary>
        public decimal MultiplyAmount { get; set; }

        /// <summary>
        /// Frecuencia de pago del impuesto.
        /// </summary>
        public int PayFrecuency { get; set; }

        /// <summary>
        /// Descripción del impuesto.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Período límite aplicable al impuesto.
        /// </summary>
        public string? LimitPeriod { get; set; }

        /// <summary>
        /// Monto límite del impuesto.
        /// </summary>
        public decimal LimitAmount { get; set; }

        /// <summary>
        /// Índice base para cálculo.
        /// </summary>
        public int IndexBase { get; set; }

        /// <summary>
        /// FK al proyecto relacionado (opcional).
        /// </summary>
        public long? ProjectRefRecID { get; set; }

        /// <summary>
        /// FK a la categoría de proyecto relacionada (opcional).
        /// </summary>
        public long? ProjectCategoryRefRecID { get; set; }

        /// <summary>
        /// FK al departamento relacionado (opcional).
        /// </summary>
        public long? DepartmentRefRecID { get; set; }

        /// <summary>
        /// Estado del impuesto (activo/inactivo).
        /// </summary>
        public bool TaxStatus { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Moneda en la que se expresa el impuesto.
        /// </summary>
        public virtual Currency CurrencyRefRec { get; set; } = null!;

        /// <summary>
        /// Departamento al que se asocia el impuesto.
        /// </summary>
        public virtual Department? DepartmentRefRec { get; set; }

        /// <summary>
        /// Relación con impuestos aplicados a empleados.
        /// </summary>
        public virtual ICollection<EmployeeTax> EmployeeTaxes { get; set; } = new List<EmployeeTax>();

        /// <summary>
        /// Categoría de proyecto vinculada.
        /// </summary>
        public virtual ProjectCategory? ProjectCategoryRefRec { get; set; }

        /// <summary>
        /// Proyecto vinculado al impuesto.
        /// </summary>
        public virtual Project? ProjectRefRec { get; set; }

        /// <summary>
        /// Detalles de cálculo del impuesto.
        /// </summary>
        public virtual ICollection<TaxDetail> TaxDetails { get; set; } = new List<TaxDetail>();
    }
}
