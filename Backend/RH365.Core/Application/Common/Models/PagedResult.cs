// ============================================================================
// Archivo: PagedResult.cs
// Capa: RH365.Core.Application
// Ruta: Application/Common/Models/PagedResult.cs
// Descripción: Contenedor genérico de resultados paginados para responses.
// ============================================================================
using System.Collections.Generic;

namespace RH365.Core.Application.Common.Models
{
    /// <summary>Resultado paginado genérico.</summary>
    public sealed class PagedResult<T>
    {
        /// <summary>Datos de la página solicitada.</summary>
        public List<T> Data { get; set; } = new();

        /// <summary>Total de registros (sin paginar).</summary>
        public int TotalCount { get; set; }

        /// <summary>Número de página actual (>=1).</summary>
        public int PageNumber { get; set; }

        /// <summary>Tamaño de página (1..100 típico).</summary>
        public int PageSize { get; set; }

        /// <summary>Total de páginas calculado.</summary>
        public int TotalPages { get; set; }

        /// <summary>¿Existe página siguiente?</summary>
        public bool HasNextPage => PageNumber < TotalPages;

        /// <summary>¿Existe página previa?</summary>
        public bool HasPreviousPage => PageNumber > 1;
    }
}
