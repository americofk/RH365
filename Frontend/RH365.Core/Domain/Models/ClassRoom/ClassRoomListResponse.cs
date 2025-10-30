// ============================================================================
// Archivo: ClassRoomListResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/ClassRoom/ClassRoomListResponse.cs
// Descripción: Response paginado para lista de salones de clases
// ============================================================================

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.ClassRoom
{
    public class ClassRoomListResponse
    {
        [JsonPropertyName("Data")]
        public List<ClassRoomResponse> Data { get; set; } = new();

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
