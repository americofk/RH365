// ============================================================================
// Archivo: CreatePayrollRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Payroll/CreatePayrollRequest.cs
// Descripción: Request para crear una nómina
// ISO 27001: Validación de entrada de datos de nómina
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Payroll
{
    public class CreatePayrollRequest
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("PayFrecuency")]
        public int PayFrecuency { get; set; }

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

        [JsonPropertyName("PayrollStatus")]
        public bool PayrollStatus { get; set; }

        [JsonPropertyName("Observations")]
        public string Observations { get; set; }
    }
}
