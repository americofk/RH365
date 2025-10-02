// ============================================================================
// Archivo: DeductionCodeDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/DeductionCode/DeductionCodeDto.cs
// Descripción:
//   - DTO de lectura para la entidad DeductionCode (dbo.DeductionCodes)
//   - Incluye claves, datos de negocio, FKs y auditoría (ISO 27001)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.DeductionCode
{
    /// <summary>
    /// DTO de salida para DeductionCode.
    /// </summary>
    public class DeductionCodeDto
    {
        // Identificadores
        public long RecID { get; set; }
        public string? ID { get; set; }

        // Datos de negocio
        public string Name { get; set; } = null!;
        public long? ProjectRefRecID { get; set; }
        public long? ProjCategoryRefRecID { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string? Description { get; set; }
        public string? LedgerAccount { get; set; }
        public long? DepartmentRefRecID { get; set; }
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
        public bool DeductionStatus { get; set; }
        public string? Observations { get; set; }

        // Auditoría ISO 27001
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}