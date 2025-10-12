// ============================================================================
// Archivo: UpdateTaxDetailRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/TaxDetail/UpdateTaxDetailRequest.cs
// Descripción:
//   - DTO de actualización parcial para TaxDetail
//   - Todos los campos son opcionales; solo se actualiza lo enviado
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.TaxDetail
{
    /// <summary>
    /// Payload para actualizar (parcial) un detalle de impuesto.
    /// </summary>
    public class UpdateTaxDetailRequest
    {
        public long? TaxRefRecID { get; set; }
        public decimal? AnnualAmountHigher { get; set; }
        public decimal? AnnualAmountNotExceed { get; set; }
        public decimal? Percent { get; set; }
        public decimal? FixedAmount { get; set; }
        public decimal? ApplicableScale { get; set; }
        public string? Observations { get; set; }
    }
}