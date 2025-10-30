// ============================================================================
// Archivo: OccupationListResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Occupation/OccupationListResponse.cs
// Descripción: Response paginado para lista de ocupaciones
// ============================================================================

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Occupation
{
    public class OccupationListResponse
    {
        [JsonPropertyName("Data")]
        public List<OccupationResponse> Data { get; set; } = new();

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
