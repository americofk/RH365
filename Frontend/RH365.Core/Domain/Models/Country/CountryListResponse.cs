// ============================================================================
// Archivo: CountryListResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Country/CountryListResponse.cs
// Descripción: Response paginado para lista de países
// Estándar: ISO 27001 - Trazabilidad de datos
// ============================================================================

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Country
{
    public class CountryListResponse
    {
        [JsonPropertyName("Data")]
        public List<CountryResponse> Data { get; set; } = new();

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
