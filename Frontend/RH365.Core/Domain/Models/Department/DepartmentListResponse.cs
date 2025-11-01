// ============================================================================
// Archivo: DepartmentListResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Department/DepartmentListResponse.cs
// Descripción: Response paginado para lista de departamentos
// Estándar: ISO 27001 - Información estructurada con paginación
// ============================================================================

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Department
{
    /// <summary>
    /// Representa una respuesta paginada de departamentos desde el API.
    /// Incluye metadatos de paginación para navegación eficiente.
    /// </summary>
    public class DepartmentListResponse
    {
        /// <summary>
        /// Lista de departamentos en la página actual
        /// </summary>
        [JsonPropertyName("Data")]
        public List<DepartmentResponse> Data { get; set; } = new();

        /// <summary>
        /// Total de registros disponibles
        /// </summary>
        [JsonPropertyName("TotalCount")]
        public int TotalCount { get; set; }

        /// <summary>
        /// Número de página actual (base 1)
        /// </summary>
        [JsonPropertyName("PageNumber")]
        public int PageNumber { get; set; }

        /// <summary>
        /// Cantidad de registros por página
        /// </summary>
        [JsonPropertyName("PageSize")]
        public int PageSize { get; set; }

        /// <summary>
        /// Total de páginas disponibles
        /// </summary>
        [JsonPropertyName("TotalPages")]
        public int TotalPages { get; set; }

        /// <summary>
        /// Indica si existe una página siguiente
        /// </summary>
        [JsonPropertyName("HasNextPage")]
        public bool HasNextPage { get; set; }

        /// <summary>
        /// Indica si existe una página anterior
        /// </summary>
        [JsonPropertyName("HasPreviousPage")]
        public bool HasPreviousPage { get; set; }
    }
}
