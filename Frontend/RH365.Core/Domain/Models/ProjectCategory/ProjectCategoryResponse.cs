// ============================================================================
// Archivo: ProjectCategoryResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/ProjectCategory/ProjectCategoryResponse.cs
// Descripción: Response para una categoría de proyecto individual
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.ProjectCategory
{
    public class ProjectCategoryResponse
    {
        [JsonPropertyName("RecID")]
        public long RecID { get; set; }

        [JsonPropertyName("ID")]
        public string ID { get; set; }

        [JsonPropertyName("CategoryName")]
        public string CategoryName { get; set; }

        [JsonPropertyName("LedgerAccount")]
        public string LedgerAccount { get; set; }

        [JsonPropertyName("ProjectRefRecID")]
        public long ProjectRefRecID { get; set; }

        [JsonPropertyName("ProjectCategoryStatus")]
        public bool ProjectCategoryStatus { get; set; }

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
