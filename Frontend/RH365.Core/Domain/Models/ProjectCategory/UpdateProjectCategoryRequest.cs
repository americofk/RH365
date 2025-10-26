// ============================================================================
// Archivo: UpdateProjectCategoryRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/ProjectCategory/UpdateProjectCategoryRequest.cs
// Descripción: Request para actualizar una categoría de proyecto
// ============================================================================

using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.ProjectCategory
{
    public class UpdateProjectCategoryRequest
    {
        [JsonPropertyName("CategoryName")]
        public string CategoryName { get; set; }

        [JsonPropertyName("LedgerAccount")]
        public string LedgerAccount { get; set; }

        [JsonPropertyName("ProjectRefRecID")]
        public long ProjectRefRecID { get; set; }

        [JsonPropertyName("ProjectCategoryStatus")]
        public bool? ProjectCategoryStatus { get; set; }
    }
}
