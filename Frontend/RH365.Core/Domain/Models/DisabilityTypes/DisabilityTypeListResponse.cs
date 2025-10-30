// ============================================================================
// Archivo: DisabilityTypeListResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/DisabilityType/DisabilityTypeListResponse.cs
// Descripción: Response paginado para lista de tipos de discapacidad
// ============================================================================

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.DisabilityType
{
    public class DisabilityTypeListResponse
    {
        [JsonPropertyName("Data")]
        public List<DisabilityTypeResponse> Data { get; set; } = new();

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
