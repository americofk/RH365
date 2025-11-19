// ============================================================================
// Archivo: EmployeeListResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Employee/EmployeeListResponse.cs
// Descripción: Response paginado para lista de empleados
// Estándar: ISO 27001 - Paginación y control de acceso a datos
// ============================================================================

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Employee
{
    public class EmployeeListResponse
    {
        [JsonPropertyName("Data")]
        public List<EmployeeResponse> Data { get; set; } = new();

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