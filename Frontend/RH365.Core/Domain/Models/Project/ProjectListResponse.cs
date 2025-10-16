// ============================================================================
// Archivo: ProjectListResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Project/ProjectListResponse.cs
// Descripción: Response paginado para lista de proyectos
// ============================================================================

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Project
{
    public class ProjectListResponse
    {
        [JsonPropertyName("Data")]
        public List<ProjectResponse> Data { get; set; } = new();

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