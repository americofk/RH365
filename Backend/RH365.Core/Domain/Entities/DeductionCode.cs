// ============================================================================
// Archivo: DeductionCode.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Payroll/DeductionCode.cs
// Descripción: Catálogo de códigos de deducción utilizados en la nómina.
//   - Define reglas, límites y parámetros de cálculo de deducciones
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Core.Domain.Entities;
using System;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un código de deducción en la nómina.
    /// </summary>
    public class DeductionCode : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único de la deducción.
        /// </summary>
        public string DeductionCode1 { get; set; } = null!;

        /// <summary>
        /// Nombre de la deducción.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Proyecto asociado (opcional).
        /// </summary>
        public string? ProjId { get; set; }

        /// <summary>
        /// Categoría del proyecto asociado (opcional).
        /// </summary>
        public string? ProjCategory { get; set; }

        /// <summary>
        /// Fecha de inicio de validez de la deducción.
        /// </summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// Fecha de finalización de validez de la deducción.
        /// </summary>
        public DateTime ValidTo { get; set; }

        /// <summary>
        /// Descripción detallada de la deducción.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Cuenta contable asociada a la deducción.
        /// </summary>
        public string? LedgerAccount { get; set; }

        /// <summary>
        /// FK al departamento responsable de la deducción.
        /// </summary>
        public long? DepartmentRefRecID { get; set; }

        public int PayrollAction { get; set; }
        public int CtbutionIndexBase { get; set; }
        public decimal CtbutionMultiplyAmount { get; set; }
        public int CtbutionPayFrecuency { get; set; }
        public int CtbutionLimitPeriod { get; set; }
        public decimal CtbutionLimitAmount { get; set; }
        public decimal CtbutionLimitAmountToApply { get; set; }
        public int DductionIndexBase { get; set; }
        public decimal DductionMultiplyAmount { get; set; }
        public int DductionPayFrecuency { get; set; }
        public int DductionLimitPeriod { get; set; }
        public decimal DductionLimitAmount { get; set; }
        public decimal DductionLimitAmountToApply { get; set; }
        public bool IsForTaxCalc { get; set; }
        public bool IsForTssCalc { get; set; }
        public bool DeductionStatus { get; set; }

        public virtual ICollection<EmployeeDeductionCode> EmployeeDeductionCodes { get; set; } = new List<EmployeeDeductionCode>();
    }
}
