// ============================================================================
// Archivo: CreateTaxDetailRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/TaxDetail/CreateTaxDetailRequest.cs
// Descripción: Request para crear un detalle de impuesto
// Estándar: ISO 27001 - Control de integridad y validación de datos
// ============================================================================

using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.TaxDetail
{
    public class CreateTaxDetailRequest
    {
        [JsonPropertyName("TaxRefRecID")]
        public long TaxRefRecID { get; set; }

        [JsonPropertyName("AnnualAmountHigher")]
        public decimal AnnualAmountHigher { get; set; }

        [JsonPropertyName("AnnualAmountNotExceed")]
        public decimal AnnualAmountNotExceed { get; set; }

        [JsonPropertyName("Percent")]
        public decimal Percent { get; set; }

        [JsonPropertyName("FixedAmount")]
        public decimal FixedAmount { get; set; }

        [JsonPropertyName("ApplicableScale")]
        public decimal ApplicableScale { get; set; }

        [JsonPropertyName("Observations")]
        public string Observations { get; set; }
    }
}
