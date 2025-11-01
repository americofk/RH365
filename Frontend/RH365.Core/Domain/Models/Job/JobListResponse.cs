// ============================================================================
// Archivo: JobListResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Job/JobListResponse.cs
// Descripción: Response paginado para lista de cargos
// Estándar: ISO 27001 - Gestión de información estructurada
// ============================================================================

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Job
{
    public class JobListResponse
    {
        [JsonPropertyName("Data")]
        public List<JobResponse> Data { get; set; } = new();

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
