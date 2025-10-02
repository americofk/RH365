// ============================================================================
// Archivo: DeductionCodeDto.cs
// Proyecto: RH365.Core.Application
// Ruta: RH365.Core.Application/Features/DTOs/DeductionCode/DeductionCodeDto.cs
// Descripción: DTO de lectura para DeductionCode.
// ============================================================================

using System;

namespace RH365.Core.Application.Features.DTOs.DeductionCode
{
    public class DeductionCodeDto
    {
        public long RecID { get; set; }
        public string ID { get; set; } = null!;          // generado por BD (DEFAULT)

        public string Name { get; set; } = null!;
        public string? ProjId { get; set; }
        public string? ProjCategory { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public string? Description { get; set; }
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

        public string? Observations { get; set; }

        // Auditoría
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
