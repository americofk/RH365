// ============================================================================
// Archivo: LoanResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Loan/LoanResponse.cs
// Descripción: Response para un préstamo individual
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Loan
{
    public class LoanResponse
    {
        [JsonPropertyName("RecID")]
        public long RecID { get; set; }

        [JsonPropertyName("ID")]
        public string ID { get; set; }

        [JsonPropertyName("LoanCode")]
        public string LoanCode { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("ValidFrom")]
        public DateTime? ValidFrom { get; set; }

        [JsonPropertyName("ValidTo")]
        public DateTime? ValidTo { get; set; }

        [JsonPropertyName("MultiplyAmount")]
        public decimal MultiplyAmount { get; set; }

        [JsonPropertyName("LedgerAccount")]
        public string LedgerAccount { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("PayFrecuency")]
        public int PayFrecuency { get; set; }

        [JsonPropertyName("IndexBase")]
        public decimal IndexBase { get; set; }

        [JsonPropertyName("DepartmentRefRecID")]
        public long? DepartmentRefRecID { get; set; }

        [JsonPropertyName("ProjCategoryRefRecID")]
        public long? ProjCategoryRefRecID { get; set; }

        [JsonPropertyName("ProjectRefRecID")]
        public long? ProjectRefRecID { get; set; }

        [JsonPropertyName("LoanStatus")]
        public bool LoanStatus { get; set; }

        [JsonPropertyName("DataareaID")]
        public string DataareaID { get; set; }

        [JsonPropertyName("CreatedBy")]
        public string CreatedBy { get; set; }

        [JsonPropertyName("CreatedOn")]
        public DateTime? CreatedOn { get; set; }

        [JsonPropertyName("ModifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonPropertyName("ModifiedOn")]
        public DateTime? ModifiedOn { get; set; }

        [JsonPropertyName("RowVersion")]
        public string RowVersion { get; set; }
    }
}
