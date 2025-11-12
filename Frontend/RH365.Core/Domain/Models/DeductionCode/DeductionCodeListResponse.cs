// ============================================================================
// Archivo: DeductionCodeListResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/DeductionCode/DeductionCodeListResponse.cs
// Descripción: 
//   - Response paginado para lista de códigos de deducción
//   - Cumplimiento ISO 27001: Control de acceso a datos paginados
//   - Incluye metadatos de paginación para navegación eficiente
// ============================================================================

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.DeductionCode
{
    /// <summary>
    /// Modelo de respuesta paginada para lista de códigos de deducción
    /// Incluye metadatos de paginación para facilitar la navegación
    /// </summary>
    public class DeductionCodeListResponse
    {
        /// <summary>
        /// Lista de códigos de deducción en la página actual
        /// </summary>
        [JsonPropertyName("Data")]
        public List<DeductionCodeResponse> Data { get; set; } = new();

        /// <summary>
        /// Número total de registros en toda la colección
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
        /// Número total de páginas disponibles
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
