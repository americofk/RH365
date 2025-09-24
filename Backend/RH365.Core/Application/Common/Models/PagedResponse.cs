// ============================================================================
// Archivo: PagedResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Common/Models/PagedResponse.cs
// Descripción: Modelo de respuesta para resultados paginados.
//   - Extiende Response con información de paginación
//   - Usado para listas grandes de datos
//   - Optimiza rendimiento y UX
// ============================================================================

using System;

namespace RH365.Core.Application.Common.Models
{
    /// <summary>
    /// Respuesta especializada para resultados paginados.
    /// Incluye metadata de paginación para navegación.
    /// </summary>
    /// <typeparam name="T">Tipo de la colección de datos paginados.</typeparam>
    public class PagedResponse<T> : Response<T>
    {
        /// <summary>
        /// Constructor para respuesta paginada.
        /// </summary>
        /// <param name="data">Datos de la página actual.</param>
        /// <param name="pageNumber">Número de página actual (base 1).</param>
        /// <param name="pageSize">Cantidad de registros por página.</param>
        public PagedResponse(T data, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Data = data;
            Succeeded = true;
            Errors = new List<string>();
        }

        /// <summary>
        /// Constructor completo con total de registros.
        /// Calcula automáticamente páginas totales.
        /// </summary>
        /// <param name="data">Datos de la página actual.</param>
        /// <param name="pageNumber">Número de página actual.</param>
        /// <param name="pageSize">Registros por página.</param>
        /// <param name="totalRecords">Total de registros en la consulta.</param>
        public PagedResponse(T data, int pageNumber, int pageSize, int totalRecords)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            Data = data;
            Succeeded = true;
            Errors = new List<string>();
        }

        /// <summary>
        /// Número de página actual (inicia en 1).
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Cantidad de registros por página.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total de páginas disponibles.
        /// Calculado automáticamente.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Total de registros en toda la consulta.
        /// No solo los de la página actual.
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Indica si existe una página anterior.
        /// </summary>
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>
        /// Indica si existe una página siguiente.
        /// </summary>
        public bool HasNextPage => PageNumber < TotalPages;

        /// <summary>
        /// Número de la primera página (siempre 1).
        /// </summary>
        public int FirstPage => 1;

        /// <summary>
        /// Número de la última página.
        /// </summary>
        public int LastPage => TotalPages;

        /// <summary>
        /// Índice del primer registro en la página actual.
        /// Base 0 para uso en queries.
        /// </summary>
        public int FirstRecordIndex => (PageNumber - 1) * PageSize;

        /// <summary>
        /// Índice del último registro en la página actual.
        /// </summary>
        public int LastRecordIndex => Math.Min(FirstRecordIndex + PageSize - 1, TotalRecords - 1);

        /// <summary>
        /// Crea metadata de paginación para URLs.
        /// Útil para APIs REST con enlaces HATEOAS.
        /// </summary>
        /// <param name="baseUrl">URL base para construir enlaces.</param>
        /// <returns>Objeto con URLs de navegación.</returns>
        public PaginationMetadata GetMetadata(string baseUrl)
        {
            return new PaginationMetadata
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
                TotalPages = TotalPages,
                TotalRecords = TotalRecords,
                FirstPageUrl = $"{baseUrl}?pageNumber=1&pageSize={PageSize}",
                LastPageUrl = $"{baseUrl}?pageNumber={TotalPages}&pageSize={PageSize}",
                NextPageUrl = HasNextPage ? $"{baseUrl}?pageNumber={PageNumber + 1}&pageSize={PageSize}" : null,
                PreviousPageUrl = HasPreviousPage ? $"{baseUrl}?pageNumber={PageNumber - 1}&pageSize={PageSize}" : null
            };
        }
    }

    /// <summary>
    /// Metadata de paginación para respuestas REST.
    /// Incluye URLs para navegación HATEOAS.
    /// </summary>
    public class PaginationMetadata
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public string? FirstPageUrl { get; set; }
        public string? LastPageUrl { get; set; }
        public string? NextPageUrl { get; set; }
        public string? PreviousPageUrl { get; set; }
    }
}