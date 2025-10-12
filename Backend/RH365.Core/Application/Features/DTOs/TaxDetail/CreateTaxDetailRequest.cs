// ============================================================================
// Archivo: CreateTaxDetailRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/TaxDetail/CreateTaxDetailRequest.cs
// Descripción:
//   - DTO de creación para TaxDetail
//   - No incluye ID/RecID (se generan en BD); auditoría la maneja el DbContext
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.TaxDetail
{
    /// <summary>
    /// Payload para crear un detalle de impuesto.
    /// </summary>
    public class CreateTaxDetailRequest
    {
        public long TaxRefRecID { get; set; }
        public decimal AnnualAmountHigher { get; set; }
        public decimal AnnualAmountNotExceed { get; set; }
        public decimal Percent { get; set; }
        public decimal FixedAmount { get; set; }
        public decimal ApplicableScale { get; set; }
        public string? Observations { get; set; }
    }
}