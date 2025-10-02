// ============================================================================
// Proyecto: RH365
// Capa:     RH365.Core.Application
// Archivo:  Application/Common/Models/PagedResult.cs
// Autor:    Wilson / RH365
// Descripción:
//   Contenedor genérico para devolver listados paginados desde los endpoints.
//   Incluye constructor SIN parámetros para permitir inicializadores de objeto.
//   No depende de ningún DTO concreto (totalmente genérico).
// ============================================================================

using System;
using System.Collections.Generic;

namespace RH365.Core.Application.Common.Models
{
    /// <summary>
    /// Representa un resultado paginado estándar.
    /// </summary>
    /// <typeparam name="T">Tipo del elemento que se pagina (DTO, VM, etc.).</typeparam>
    public sealed class PagedResult<T>
    {
        /// <summary>Registros de la página.</summary>
        public IEnumerable<T> Data { get; set; }

        /// <summary>Total de registros de la consulta (sin paginar).</summary>
        public int TotalCount { get; set; }

        /// <summary>Número de página actual (1-based).</summary>
        public int PageNumber { get; set; }

        /// <summary>Tamaño de página.</summary>
        public int PageSize { get; set; }

        /// <summary>Total de páginas.</summary>
        public int TotalPages { get; set; }

        /// <summary>¿Existe una página siguiente?</summary>
        public bool HasNextPage => PageNumber < TotalPages;

        /// <summary>¿Existe una página previa?</summary>
        public bool HasPreviousPage => PageNumber > 1;

        // --------------------------------------------------------------------
        // Constructor sin parámetros: habilita new PagedResult<T> { ... }
        // --------------------------------------------------------------------
        public PagedResult()
        {
            Data = Array.Empty<T>();
            TotalCount = 0;
            PageNumber = 1;
            PageSize = 0;
            TotalPages = 0;
        }

        // --------------------------------------------------------------------
        // Constructor completo por conveniencia.
        // Calcula TotalPages en base a totalCount y pageSize.
        // --------------------------------------------------------------------
        public PagedResult(IEnumerable<T> data, int totalCount, int pageNumber, int pageSize)
        {
            Data = data ?? Array.Empty<T>();
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = pageSize <= 0 ? 0 : (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
