// ============================================================================
// Archivo: EmployeeContactInfoListResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/EmployeeContactInfo/EmployeeContactInfoListResponse.cs
// Descripcion: Response paginado para lista de informacion de contacto
// Estandar: ISO 27001 - Paginacion estandar del sistema
// ============================================================================

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.EmployeeContactInfo
{
    /// <summary>
    /// Response paginado para lista de informacion de contacto de empleados
    /// Implementa paginacion estandar del sistema
    /// </summary>
    public class EmployeeContactInfoListResponse
    {
        /// <summary>
        /// Lista de contactos de empleados en la pagina actual
        /// </summary>
        [JsonPropertyName("Data")]
        public List<EmployeeContactInfoResponse> Data { get; set; } = new();

        /// <summary>
        /// Numero total de registros en la base de datos
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
        /// Numero total de paginas disponibles
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
