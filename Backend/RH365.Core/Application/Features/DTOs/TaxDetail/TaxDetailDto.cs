// ============================================================================
// Archivo: TaxDetailDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/TaxDetail/TaxDetailDto.cs
// Descripción:
//   - DTO de lectura para la entidad TaxDetail
//   - Incluye claves, datos y auditoría (ISO 27001)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.TaxDetail
{
    /// <summary>
    /// DTO de salida para TaxDetail.
    /// </summary>
    public class TaxDetailDto
    {
        public long RecID { get; set; }
        public string? ID { get; set; }
        public long TaxRefRecID { get; set; }
        public decimal AnnualAmountHigher { get; set; }
        public decimal AnnualAmountNotExceed { get; set; }
        public decimal Percent { get; set; }
        public decimal FixedAmount { get; set; }
        public decimal ApplicableScale { get; set; }
        public string? Observations { get; set; }
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}