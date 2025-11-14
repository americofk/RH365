// ============================================================================
// Archivo: PayCycleResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/PayCycle/PayCycleResponse.cs
// Descripción: Response para un ciclo de pago individual
// ISO 27001: Estructura de datos de salida con trazabilidad completa
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.PayCycle
{
    public class PayCycleResponse
    {
        [JsonPropertyName("RecID")]
        public long RecID { get; set; }

        [JsonPropertyName("ID")]
        public string ID { get; set; }

        [JsonPropertyName("PayrollRefRecID")]
        public long PayrollRefRecID { get; set; }

        [JsonPropertyName("PayrollName")]
        public string PayrollName { get; set; }

        [JsonPropertyName("PeriodStartDate")]
        public DateTime PeriodStartDate { get; set; }

        [JsonPropertyName("PeriodEndDate")]
        public DateTime PeriodEndDate { get; set; }

        [JsonPropertyName("DefaultPayDate")]
        public DateTime DefaultPayDate { get; set; }

        [JsonPropertyName("PayDate")]
        public DateTime PayDate { get; set; }

        [JsonPropertyName("AmountPaidPerPeriod")]
        public decimal AmountPaidPerPeriod { get; set; }

        [JsonPropertyName("StatusPeriod")]
        public int StatusPeriod { get; set; }

        [JsonPropertyName("StatusPeriodName")]
        public string StatusPeriodName { get; set; }

        [JsonPropertyName("IsForTax")]
        public bool IsForTax { get; set; }

        [JsonPropertyName("IsForTss")]
        public bool IsForTss { get; set; }

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
