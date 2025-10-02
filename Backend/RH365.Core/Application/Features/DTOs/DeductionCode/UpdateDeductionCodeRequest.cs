// ============================================================================
// Archivo: UpdateDeductionCodeRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/DeductionCode/UpdateDeductionCodeRequest.cs
// Descripción:
//   - DTO de actualización parcial para DeductionCode (dbo.DeductionCodes)
//   - Todos los campos son opcionales; solo se actualiza lo enviado
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.DeductionCode
{
    /// <summary>
    /// Payload para actualizar (parcial) un código de deducción.
    /// </summary>
    public class UpdateDeductionCodeRequest
    {
        public string? Name { get; set; }
        public long? ProjectRefRecID { get; set; }
        public long? ProjCategoryRefRecID { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string? Description { get; set; }
        public string? LedgerAccount { get; set; }
        public long? DepartmentRefRecID { get; set; }
        public int? PayrollAction { get; set; }

        // Parámetros de contribución
        public int? CtbutionIndexBase { get; set; }
        public decimal? CtbutionMultiplyAmount { get; set; }
        public int? CtbutionPayFrecuency { get; set; }
        public int? CtbutionLimitPeriod { get; set; }
        public decimal? CtbutionLimitAmount { get; set; }
        public decimal? CtbutionLimitAmountToApply { get; set; }

        // Parámetros de deducción
        public int? DductionIndexBase { get; set; }
        public decimal? DductionMultiplyAmount { get; set; }
        public int? DductionPayFrecuency { get; set; }
        public int? DductionLimitPeriod { get; set; }
        public decimal? DductionLimitAmount { get; set; }
        public decimal? DductionLimitAmountToApply { get; set; }

        // Banderas
        public bool? IsForTaxCalc { get; set; }
        public bool? IsForTssCalc { get; set; }
        public bool? DeductionStatus { get; set; }
        public string? Observations { get; set; }
    }
}