// ============================================================================
// Archivo: Loan.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Finance/Loan.cs
// Descripción: Entidad que representa los tipos de préstamos disponibles.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Permite configurar montos, períodos, frecuencia de pago y relación con proyectos
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Core.Domain.Entities;
using System;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un tipo de préstamo en el sistema.
    /// </summary>
    public class Loan : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único del préstamo.
        /// </summary>
        public string LoanCode { get; set; } = null!;

        /// <summary>
        /// Nombre descriptivo del préstamo.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Fecha de inicio de vigencia del préstamo.
        /// </summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// Fecha de finalización de vigencia del préstamo.
        /// </summary>
        public DateTime ValidTo { get; set; }

        /// <summary>
        /// Monto multiplicador aplicado al préstamo.
        /// </summary>
        public decimal MultiplyAmount { get; set; }

        /// <summary>
        /// Cuenta contable asociada (si aplica).
        /// </summary>
        public string? LedgerAccount { get; set; }

        /// <summary>
        /// Descripción adicional del préstamo.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Frecuencia de pago del préstamo.
        /// </summary>
        public int PayFrecuency { get; set; }

        /// <summary>
        /// Índice base utilizado para el cálculo.
        /// </summary>
        public int IndexBase { get; set; }

        /// <summary>
        /// FK al departamento relacionado (si aplica).
        /// </summary>
        public long? DepartmentRefRecID { get; set; }

        /// <summary>
        /// FK a la categoría de proyecto relacionada (si aplica).
        /// </summary>
        public long? ProjCategoryRefRecID { get; set; }

        /// <summary>
        /// FK al proyecto relacionado (si aplica).
        /// </summary>
        public long? ProjectRefRecID { get; set; }

        /// <summary>
        /// Estado del préstamo (activo/inactivo).
        /// </summary>
        public bool LoanStatus { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Departamento relacionado con el préstamo.
        /// </summary>
        public virtual Department? DepartmentRefRec { get; set; }

        /// <summary>
        /// Historial de préstamos de empleados relacionados.
        /// </summary>
        public virtual ICollection<EmployeeLoanHistory> EmployeeLoanHistories { get; set; } = new List<EmployeeLoanHistory>();

        /// <summary>
        /// Relación de préstamos otorgados a empleados.
        /// </summary>
        public virtual ICollection<EmployeeLoan> EmployeeLoans { get; set; } = new List<EmployeeLoan>();

        /// <summary>
        /// Categoría de proyecto relacionada.
        /// </summary>
        public virtual ProjectCategory? ProjCategoryRefRec { get; set; }

        /// <summary>
        /// Proyecto relacionado.
        /// </summary>
        public virtual Project? ProjectRefRec { get; set; }
    }
}
