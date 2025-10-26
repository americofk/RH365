// ============================================================================
// Archivo: ProjectCategoryListResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/ProjectCategory/ProjectCategoryListResponse.cs
// Descripción: Response paginado para lista de categorías de proyecto
// ============================================================================

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.ProjectCategory
{
    public class ProjectCategoryListResponse
    {
        [JsonPropertyName("Data")]
        public List<ProjectCategoryResponse> Data { get; set; } = new();

        [JsonPropertyName("TotalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("PageNumber")]
        public int PageNumber { get; set; }

        [JsonPropertyName("PageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("TotalPages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("HasNextPage")]
        public bool HasNextPage { get; set; }

        [JsonPropertyName("HasPreviousPage")]
        public bool HasPreviousPage { get; set; }
    }
}
