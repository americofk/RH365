// ============================================================================
// Archivo: EarningCodeResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/EarningCode/EarningCodeResponse.cs
// Descripción: Response para un código de nómina individual
// Estándar: ISO 27001 - Trazabilidad completa de información de nómina
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.EarningCode
{
    public class EarningCodeResponse
    {
        [JsonPropertyName("RecID")]
        public long RecID { get; set; }

        [JsonPropertyName("ID")]
        public string ID { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("IsTSS")]
        public bool IsTSS { get; set; }

        [JsonPropertyName("IsISR")]
        public bool IsISR { get; set; }

        [JsonPropertyName("ProjectRefRecID")]
        public long? ProjectRefRecID { get; set; }

        [JsonPropertyName("ProjectName")]
        public string ProjectName { get; set; }

        [JsonPropertyName("ProjCategoryRefRecID")]
        public long? ProjCategoryRefRecID { get; set; }

        [JsonPropertyName("ProjCategoryName")]
        public string ProjCategoryName { get; set; }

        [JsonPropertyName("ValidFrom")]
        public DateTime ValidFrom { get; set; }

        [JsonPropertyName("ValidTo")]
        public DateTime ValidTo { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("IndexBase")]
        public int IndexBase { get; set; }

        [JsonPropertyName("MultiplyAmount")]
        public decimal MultiplyAmount { get; set; }

        [JsonPropertyName("LedgerAccount")]
        public string LedgerAccount { get; set; }

        [JsonPropertyName("DepartmentRefRecID")]
        public long? DepartmentRefRecID { get; set; }

        [JsonPropertyName("DepartmentName")]
        public string DepartmentName { get; set; }

        [JsonPropertyName("EarningCodeStatus")]
        public bool EarningCodeStatus { get; set; }

        [JsonPropertyName("IsExtraHours")]
        public bool IsExtraHours { get; set; }

        [JsonPropertyName("IsRoyaltyPayroll")]
        public bool IsRoyaltyPayroll { get; set; }

        [JsonPropertyName("IsUseDGT")]
        public bool IsUseDGT { get; set; }

        [JsonPropertyName("IsHoliday")]
        public bool IsHoliday { get; set; }

        [JsonPropertyName("WorkFrom")]
        public TimeSpan? WorkFrom { get; set; }

        [JsonPropertyName("WorkTo")]
        public TimeSpan? WorkTo { get; set; }

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
