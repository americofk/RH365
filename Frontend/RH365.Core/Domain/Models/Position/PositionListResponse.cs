// ============================================================================
// Archivo: PositionListResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Position/PositionListResponse.cs
// Descripción: Response paginado para lista de posiciones
// Estándar: ISO 27001 - Control A.12.1.1 (Procedimientos operacionales documentados)
// ============================================================================

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Position
{
    public class PositionListResponse
    {
        [JsonPropertyName("Data")]
        public List<PositionResponse> Data { get; set; } = new();

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
