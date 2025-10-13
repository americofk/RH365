// ============================================================================
// Archivo: DeductionCode.cs (CORREGIDO)
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Payroll/DeductionCode.cs
// Descripción:
//   - Catálogo de códigos de deducción utilizados en la nómina.
//   - Define relaciones FK con Project, ProjectCategory y Department mediante RecID.
//   - Define reglas, límites y parámetros de cálculo (contribución y deducción).
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - [NotMapped] en navegación inversa para evitar shadow properties
// ============================================================================

using RH365.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un tipo de deducción en la nómina.
    /// </summary>
    public class DeductionCode : AuditableCompanyEntity
    {
        /// <summary>
        /// Nombre de la deducción (identificador funcional).
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// FK al proyecto asociado (opcional).
        /// </summary>
        public long? ProjectRefRecID { get; set; }

        /// <summary>
        /// FK a la categoría del proyecto asociado (opcional).
        /// </summary>
        public long? ProjCategoryRefRecID { get; set; }

        /// <summary>
        /// Vigente desde.
        /// </summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// Vigente hasta.
        /// </summary>
        public DateTime ValidTo { get; set; }

        /// <summary>
        /// Descripción de la deducción (opcional).
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Cuenta contable asociada (opcional).
        /// </summary>
        public string? LedgerAccount { get; set; }

        /// <summary>
        /// FK al departamento responsable (opcional).
        /// </summary>
        public long? DepartmentRefRecID { get; set; }

        /// <summary>
        /// Acción de nómina (enum/int).
        /// </summary>
        public int PayrollAction { get; set; }

        // -------------------------
        // Parámetros de Contribución
        // -------------------------
        public int CtbutionIndexBase { get; set; }
        public decimal CtbutionMultiplyAmount { get; set; }
        public int CtbutionPayFrecuency { get; set; }
        public int CtbutionLimitPeriod { get; set; }
        public decimal CtbutionLimitAmount { get; set; }
        public decimal CtbutionLimitAmountToApply { get; set; }

        // -------------------------
        // Parámetros de Deducción
        // -------------------------
        public int DductionIndexBase { get; set; }
        public decimal DductionMultiplyAmount { get; set; }
        public int DductionPayFrecuency { get; set; }
        public int DductionLimitPeriod { get; set; }
        public decimal DductionLimitAmount { get; set; }
        public decimal DductionLimitAmountToApply { get; set; }

        /// <summary>
        /// Participa en cálculo de impuestos.
        /// </summary>
        public bool IsForTaxCalc { get; set; }

        /// <summary>
        /// Participa en cálculo TSS.
        /// </summary>
        public bool IsForTssCalc { get; set; }

        /// <summary>
        /// Estado (activo/inactivo).
        /// </summary>
        public bool DeductionStatus { get; set; } = true;

        // -------------------------
        // Navegación
        // -------------------------
        public virtual Project? ProjectRefRec { get; set; }
        public virtual ProjectCategory? ProjCategoryRefRec { get; set; }
        public virtual Department? DepartmentRefRec { get; set; }

        /// <summary>
        /// Colección de deducciones de empleados.
        /// [NotMapped] para evitar inferencia automática de FK incorrecta.
        /// </summary>
        [NotMapped]
        public virtual ICollection<EmployeeDeductionCode> EmployeeDeductionCodes { get; set; } = new List<EmployeeDeductionCode>();
    }
}