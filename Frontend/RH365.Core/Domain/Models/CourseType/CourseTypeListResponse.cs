// ============================================================================
// Archivo: CourseTypeListResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/CourseType/CourseTypeListResponse.cs
// Descripción: Response paginado para lista de tipos de curso
// Estándar: ISO 27001 - Trazabilidad de datos
// ============================================================================

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.CourseType
{
    public class CourseTypeListResponse
    {
        [JsonPropertyName("Data")]
        public List<CourseTypeResponse> Data { get; set; } = new();

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
