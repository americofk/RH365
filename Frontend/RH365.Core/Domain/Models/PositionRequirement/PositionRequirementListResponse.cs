// ============================================================================
// Archivo: PositionRequirementListResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/PositionRequirement/PositionRequirementListResponse.cs
// Descripción: Response paginado para lista de requisitos de posición
// Estándar: ISO 27001 - Control A.12.1.1 (Procedimientos operacionales documentados)
// ============================================================================

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.PositionRequirement
{
    public class PositionRequirementListResponse
    {
        [JsonPropertyName("Data")]
        public List<PositionRequirementResponse> Data { get; set; } = new();

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