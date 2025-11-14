// ============================================================================
// Archivo: CreatePayCycleRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/PayCycle/CreatePayCycleRequest.cs
// Descripción: Request para crear un ciclo de pago
// ISO 27001: Validación de entrada de datos de ciclo de pago
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.PayCycle
{
    public class CreatePayCycleRequest
    {
        [JsonPropertyName("PayrollRefRecID")]
        public long PayrollRefRecID { get; set; }

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

        [JsonPropertyName("IsForTax")]
        public bool IsForTax { get; set; }

        [JsonPropertyName("IsForTss")]
        public bool IsForTss { get; set; }

        [JsonPropertyName("Observations")]
        public string Observations { get; set; }
    }
}
