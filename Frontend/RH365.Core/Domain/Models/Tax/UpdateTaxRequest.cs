// ============================================================================
// Archivo: UpdateTaxRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Tax/UpdateTaxRequest.cs
// Descripción: Request para actualizar un impuesto
// Estándar: ISO 27001 - Control de cambios y gestión de modificaciones
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Tax
{
    public class UpdateTaxRequest
    {
        [JsonPropertyName("TaxCode")]
        public string TaxCode { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("LedgerAccount")]
        public string LedgerAccount { get; set; }

        [JsonPropertyName("ValidFrom")]
        public DateTime? ValidFrom { get; set; }

        [JsonPropertyName("ValidTo")]
        public DateTime? ValidTo { get; set; }

        [JsonPropertyName("CurrencyRefRecID")]
        public long? CurrencyRefRecID { get; set; }

        [JsonPropertyName("MultiplyAmount")]
        public decimal? MultiplyAmount { get; set; }

        [JsonPropertyName("PayFrecuency")]
        public int? PayFrecuency { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("LimitPeriod")]
        public string LimitPeriod { get; set; }

        [JsonPropertyName("LimitAmount")]
        public decimal? LimitAmount { get; set; }

        [JsonPropertyName("IndexBase")]
        public decimal? IndexBase { get; set; }

        [JsonPropertyName("ProjectRefRecID")]
        public long? ProjectRefRecID { get; set; }

        [JsonPropertyName("ProjectCategoryRefRecID")]
        public long? ProjectCategoryRefRecID { get; set; }

        [JsonPropertyName("DepartmentRefRecID")]
        public long? DepartmentRefRecID { get; set; }

        [JsonPropertyName("TaxStatus")]
        public bool? TaxStatus { get; set; }

        [JsonPropertyName("Observations")]
        public string Observations { get; set; }
    }
}
