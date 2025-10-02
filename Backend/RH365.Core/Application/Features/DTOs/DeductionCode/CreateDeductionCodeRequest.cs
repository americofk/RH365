// ============================================================================
// Archivo: CreateDeductionCodeRequest.cs
// Proyecto: RH365.Core.Application
// Ruta: RH365.Core.Application/Features/DTOs/DeductionCode/CreateDeductionCodeRequest.cs
// Descripción: DTO de creación para DeductionCode.
//   - No incluye RecID ni ID (ambos generados por BD)
// ============================================================================

using System;
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.DeductionCode
{
    public class CreateDeductionCodeRequest
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(50)]
        public string? ProjId { get; set; }

        [MaxLength(50)]
        public string? ProjCategory { get; set; }

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        [MaxLength(50)]
        public string? LedgerAccount { get; set; }

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

        [MaxLength(500)]
        public string? Observations { get; set; }
    }
}
