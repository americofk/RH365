// ============================================================================
// Archivo: TaxDetailResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/TaxDetail/TaxDetailResponse.cs
// Descripción: Response para un detalle de impuesto individual
// Estándar: ISO 27001 - Trazabilidad y auditoría de registros
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.TaxDetail
{
    public class TaxDetailResponse
    {
        [JsonPropertyName("RecID")]
        public long RecID { get; set; }

        [JsonPropertyName("ID")]
        public string ID { get; set; }

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

        [JsonPropertyName("DataareaID")]
        public string DataareaID { get; set; }

        [JsonPropertyName("CreatedBy")]
        public string CreatedBy { get; set; }

        [JsonPropertyName("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [JsonPropertyName("ModifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonPropertyName("ModifiedOn")]
        public DateTime? ModifiedOn { get; set; }

        [JsonPropertyName("RowVersion")]
        public byte[] RowVersion { get; set; }
    }
}
