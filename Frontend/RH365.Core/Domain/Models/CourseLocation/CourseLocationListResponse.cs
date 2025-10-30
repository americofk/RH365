// ============================================================================
// Archivo: CourseLocationListResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/CourseLocation/CourseLocationListResponse.cs
// Descripción: Response paginado para lista de ubicaciones de curso
// ISO 27001: Estructura de respuesta con metadatos de paginación
// ============================================================================

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.CourseLocation
{
    public class CourseLocationListResponse
    {
        [JsonPropertyName("Data")]
        public List<CourseLocationResponse> Data { get; set; } = new();

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
