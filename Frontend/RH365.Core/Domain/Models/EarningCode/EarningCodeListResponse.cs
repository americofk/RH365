// ============================================================================
// Archivo: EarningCodeListResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/EarningCode/EarningCodeListResponse.cs
// Descripción: Response paginado para lista de códigos de nómina
// Estándar: ISO 27001 - Gestión de información de nómina con paginación
// ============================================================================

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.EarningCode
{
    public class EarningCodeListResponse
    {
        [JsonPropertyName("Data")]
        public List<EarningCodeResponse> Data { get; set; } = new();

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
