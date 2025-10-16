// ============================================================================
// Archivo: UserGridViewListResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/UserGridViews/UserGridViewListResponse.cs
// Descripción: Response paginado para lista de vistas
// ============================================================================

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.UserGridViews
{
    public class UserGridViewListResponse
    {
        [JsonPropertyName("Data")]
        public List<UserGridViewResponse> Data { get; set; } = new();

        [JsonPropertyName("TotalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("PageNumber")]
        public int PageNumber { get; set; }

        [JsonPropertyName("PageSize")]
        public int PageSize { get; set; }
    }
}