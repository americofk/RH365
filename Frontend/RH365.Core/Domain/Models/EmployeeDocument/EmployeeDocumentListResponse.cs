// ============================================================================
// Archivo: EmployeeDocumentListResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/EmployeeDocument/EmployeeDocumentListResponse.cs
// Descripcion: 
//   - Response paginado para lista de documentos de empleados
//   - Soporta paginacion y metadatos de navegacion
//   - Facilita la carga de grandes volumenes de documentos
// Estandar: ISO 27001 - Consulta eficiente de registros documentales
// ============================================================================

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.EmployeeDocument
{
    public class EmployeeDocumentListResponse
    {
        /// <summary>
        /// Lista de documentos de empleados
        /// </summary>
        [JsonPropertyName("Data")]
        public List<EmployeeDocumentResponse> Data { get; set; } = new();

        /// <summary>
        /// Total de registros sin paginacion
        /// </summary>
        [JsonPropertyName("TotalCount")]
        public int TotalCount { get; set; }

        /// <summary>
        /// Numero de pagina actual
        /// </summary>
        [JsonPropertyName("PageNumber")]
        public int PageNumber { get; set; }

        /// <summary>
        /// Cantidad de registros por pagina
        /// </summary>
        [JsonPropertyName("PageSize")]
        public int PageSize { get; set; }

        /// <summary>
        /// Total de paginas disponibles
        /// </summary>
        [JsonPropertyName("TotalPages")]
        public int TotalPages { get; set; }

        /// <summary>
        /// Indica si existe una pagina siguiente
        /// </summary>
        [JsonPropertyName("HasNextPage")]
        public bool HasNextPage { get; set; }

        /// <summary>
        /// Indica si existe una pagina anterior
        /// </summary>
        [JsonPropertyName("HasPreviousPage")]
        public bool HasPreviousPage { get; set; }
    }
}
