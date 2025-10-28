// ============================================================================
// Archivo: EducationLevelListResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/EducationLevel/EducationLevelListResponse.cs
// Descripción:
//   DTO de salida para listados paginados de Niveles Educativos.
//   Estructura compatible con DataTables/TS (propiedad Data + metadatos de página).
// Notas:
//   - La colección Data contiene elementos de EducationLevelResponse.
//   - Los contadores y banderas de paginación siguen el contrato del backend.
// ============================================================================

using System.Collections.Generic;

namespace RH365.Core.Domain.Models.EducationLevel
{
    /// <summary>
    /// Respuesta paginada de Niveles Educativos.
    /// </summary>
    public sealed class EducationLevelListResponse
    {
        /// <summary>
        /// Registros de la página actual.
        /// </summary>
        public List<EducationLevelResponse> Data { get; set; } = new();

        /// <summary>
        /// Cantidad total de registros (sin paginar).
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Número de página actual (1..N).
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Tamaño de página solicitado.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Cantidad total de páginas.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Indica si existe una página siguiente.
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        /// Indica si existe una página anterior.
        /// </summary>
        public bool HasPreviousPage { get; set; }
    }
}
