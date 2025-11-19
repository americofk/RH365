// ============================================================================
// Archivo: EmployeeAddressListResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/EmployeeAddress/EmployeeAddressListResponse.cs
// Descripcion: Response paginado para lista de direcciones de empleados
// Estandar: ISO 27001 - Control de acceso a datos paginados
// ============================================================================

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.EmployeeAddress
{
    /// <summary>
    /// Modelo de respuesta paginada para direcciones de empleados
    /// ISO 27001: Control de volumen de datos y paginacion segura
    /// </summary>
    public class EmployeeAddressListResponse
    {
        /// <summary>
        /// Lista de direcciones en la pagina actual
        /// </summary>
        [JsonPropertyName("Data")]
        public List<EmployeeAddressResponse> Data { get; set; } = new();

        /// <summary>
        /// Total de registros en la base de datos
        /// </summary>
        [JsonPropertyName("TotalCount")]
        public int TotalCount { get; set; }

        /// <summary>
        /// Numero de pagina actual (base 1)
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

        /// <summary>
        /// Filtros aplicados a la busqueda (para auditoria)
        /// ISO 27001: Registro de criterios de busqueda
        /// </summary>
        [JsonPropertyName("AppliedFilters")]
        public Dictionary<string, string> AppliedFilters { get; set; } = new();

        /// <summary>
        /// Timestamp de la consulta
        /// ISO 27001: Trazabilidad temporal de consultas
        /// </summary>
        [JsonPropertyName("QueryTimestamp")]
        public System.DateTime QueryTimestamp { get; set; } = System.DateTime.UtcNow;
    }
}