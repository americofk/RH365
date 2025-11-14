// ============================================================================
// Archivo: PayrollListResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Payroll/PayrollListResponse.cs
// Descripción: Response paginado para lista de nóminas
// ISO 27001: Estructura de datos de salida paginada para optimización de consultas
// ============================================================================

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Payroll
{
    public class PayrollListResponse
    {
        [JsonPropertyName("Data")]
        public List<PayrollResponse> Data { get; set; } = new();

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
