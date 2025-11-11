// ============================================================================
// Archivo: TaxDetailListResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/TaxDetail/TaxDetailListResponse.cs
// Descripción: Response paginado para lista de detalles de impuestos
// Estándar: ISO 27001 - Control de acceso y confidencialidad de datos
// ============================================================================

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.TaxDetail
{
    public class TaxDetailListResponse
    {
        [JsonPropertyName("Data")]
        public List<TaxDetailResponse> Data { get; set; } = new();

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
