// ============================================================================
// Archivo: PagedResult.cs
// Capa: RH365.Core.Application
// Ruta: Application/Common/Models/PagedResult.cs
// Descripción: Contenedor genérico de resultados paginados para responses.
// ============================================================================
using System.Collections.Generic;
using RH365.Core.Application.DTOs.Position;
using RH365.Core.Application.Features.DTOs.DeductionCode;

namespace RH365.Core.Application.Common.Models
{
    /// <summary>Resultado paginado genérico.</summary>
    public sealed class PagedResult<T>
    {
        private List<DeductionCodeDto> data;
        private List<PositionDto> items;

        public PagedResult(List<DeductionCodeDto> data, int totalCount, int pageNumber, int pageSize)
        {
            this.data = data;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public PagedResult(List<PositionDto> items, int totalCount, int pageNumber, int pageSize)
        {
            this.items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

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
