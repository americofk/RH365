// ============================================================================
// Archivo: PayrollResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Payroll/PayrollResponse.cs
// Descripción: Response para una nómina individual
// ISO 27001: Estructura de datos de salida con trazabilidad completa
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Payroll
{
    public class PayrollResponse
    {
        [JsonPropertyName("RecID")]
        public long RecID { get; set; }

        [JsonPropertyName("ID")]
        public string ID { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("PayFrecuency")]
        public int PayFrecuency { get; set; }

        [JsonPropertyName("PayFrecuencyName")]
        public string PayFrecuencyName { get; set; }

        [JsonPropertyName("ValidFrom")]
        public DateTime ValidFrom { get; set; }

        [JsonPropertyName("ValidTo")]
        public DateTime ValidTo { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("IsRoyaltyPayroll")]
        public bool IsRoyaltyPayroll { get; set; }

        [JsonPropertyName("IsForHourPayroll")]
        public bool IsForHourPayroll { get; set; }

        [JsonPropertyName("BankSecuence")]
        public int BankSecuence { get; set; }

        [JsonPropertyName("CurrencyRefRecID")]
        public long CurrencyRefRecID { get; set; }

        [JsonPropertyName("CurrencyName")]
        public string CurrencyName { get; set; }

        [JsonPropertyName("PayrollStatus")]
        public bool PayrollStatus { get; set; }

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
