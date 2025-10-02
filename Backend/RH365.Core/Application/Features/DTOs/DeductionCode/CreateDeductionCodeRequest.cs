// ============================================================================
// Archivo: CreateDeductionCodeRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/DeductionCode/CreateDeductionCodeRequest.cs
// Descripción:
//   - DTO de creación para DeductionCode (dbo.DeductionCodes)
//   - No incluye ID/RecID (se generan en BD); auditoría la maneja el DbContext
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.DeductionCode
{
    /// <summary>
    /// Payload para crear un código de deducción.
    /// </summary>
    public class CreateDeductionCodeRequest
    {
        /// <summary>Nombre de la deducción.</summary>
        public string Name { get; set; } = null!;

        /// <summary>FK al proyecto asociado (opcional).</summary>
        public long? ProjectRefRecID { get; set; }

        /// <summary>FK a la categoría del proyecto (opcional).</summary>
        public long? ProjCategoryRefRecID { get; set; }

        /// <summary>Vigente desde.</summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>Vigente hasta.</summary>
        public DateTime ValidTo { get; set; }

        /// <summary>Descripción (opcional).</summary>
        public string? Description { get; set; }

        /// <summary>Cuenta contable (opcional).</summary>
        public string? LedgerAccount { get; set; }

        /// <summary>FK al departamento (opcional).</summary>
        public long? DepartmentRefRecID { get; set; }

        /// <summary>Acción de nómina.</summary>
        public int PayrollAction { get; set; }

        // Parámetros de contribución
        public int CtbutionIndexBase { get; set; }
        public decimal CtbutionMultiplyAmount { get; set; }
        public int CtbutionPayFrecuency { get; set; }
        public int CtbutionLimitPeriod { get; set; }
        public decimal CtbutionLimitAmount { get; set; }
        public decimal CtbutionLimitAmountToApply { get; set; }

        // Parámetros de deducción
        public int DductionIndexBase { get; set; }
        public decimal DductionMultiplyAmount { get; set; }
        public int DductionPayFrecuency { get; set; }
        public int DductionLimitPeriod { get; set; }
        public decimal DductionLimitAmount { get; set; }
        public decimal DductionLimitAmountToApply { get; set; }

        // Banderas
        public bool IsForTaxCalc { get; set; }
        public bool IsForTssCalc { get; set; }
        public bool DeductionStatus { get; set; } = true;

        /// <summary>Observaciones (opcional).</summary>
        public string? Observations { get; set; }
    }
}