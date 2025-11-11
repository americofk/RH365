// ============================================================================
// Archivo: TaxResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Tax/TaxResponse.cs
// Descripción: Response para un impuesto individual
// Estándar: ISO 27001 - Trazabilidad y auditoría de registros
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Tax
{
    public class TaxResponse
    {
        [JsonPropertyName("RecID")]
        public long RecID { get; set; }

        [JsonPropertyName("ID")]
        public string ID { get; set; }

        [JsonPropertyName("TaxCode")]
        public string TaxCode { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("LedgerAccount")]
        public string LedgerAccount { get; set; }

        [JsonPropertyName("ValidFrom")]
        public DateTime ValidFrom { get; set; }

        [JsonPropertyName("ValidTo")]
        public DateTime ValidTo { get; set; }

        [JsonPropertyName("CurrencyRefRecID")]
        public long? CurrencyRefRecID { get; set; }

        [JsonPropertyName("MultiplyAmount")]
        public decimal MultiplyAmount { get; set; }

        [JsonPropertyName("PayFrecuency")]
        public int PayFrecuency { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("LimitPeriod")]
        public string LimitPeriod { get; set; }

        [JsonPropertyName("LimitAmount")]
        public decimal LimitAmount { get; set; }

        [JsonPropertyName("IndexBase")]
        public decimal IndexBase { get; set; }

        [JsonPropertyName("ProjectRefRecID")]
        public long? ProjectRefRecID { get; set; }

        [JsonPropertyName("ProjectCategoryRefRecID")]
        public long? ProjectCategoryRefRecID { get; set; }

        [JsonPropertyName("DepartmentRefRecID")]
        public long? DepartmentRefRecID { get; set; }

        [JsonPropertyName("TaxStatus")]
        public bool TaxStatus { get; set; }

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
