// ============================================================================
// Archivo: PaginationFilter.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Common/Filters/PaginationFilter.cs
// Descripción: Filtro para manejo de paginación en consultas.
//   - Control de páginas y tamaño
//   - Validación de límites
//   - Base para consultas paginadas
// ============================================================================

using System;

namespace RH365.Core.Application.Common.Filters
{
    /// <summary>
    /// Filtro de paginación para consultas de listas.
    /// Controla qué página y cuántos registros retornar.
    /// </summary>
    public class PaginationFilter
    {
        private int _pageNumber;
        private int _pageSize;

        /// <summary>
        /// Número de página solicitada (base 1).
        /// Mínimo valor: 1.
        /// </summary>
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }

        /// <summary>
        /// Cantidad de registros por página.
        /// Mínimo: 1, Máximo: 100.
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 100 ? 100 : value < 1 ? 10 : value;
        }

        /// <summary>
        /// Constructor por defecto.
        /// Página 1, 10 registros.
        /// </summary>
        public PaginationFilter()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        /// <summary>
        /// Constructor con valores específicos.
        /// </summary>
        /// <param name="pageNumber">Número de página deseada.</param>
        /// <param name="pageSize">Registros por página.</param>
        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        /// <summary>
        /// Calcula el número de registros a saltar.
        /// Usado en queries LINQ Skip().
        /// </summary>
        public int Skip => (PageNumber - 1) * PageSize;

        /// <summary>
        /// Número de registros a tomar.
        /// Usado en queries LINQ Take().
        /// </summary>
        public int Take => PageSize;

        /// <summary>
        /// Valida si los valores de paginación son válidos.
        /// </summary>
        public bool IsValid => PageNumber >= 1 && PageSize >= 1 && PageSize <= 100;

        /// <summary>
        /// Crea un filtro con valores por defecto válidos.
        /// </summary>
        public static PaginationFilter Default() => new PaginationFilter(1, 10);

        /// <summary>
        /// Crea un filtro para obtener todos los registros.
        /// Usar con precaución en tablas grandes.
        /// </summary>
        public static PaginationFilter All() => new PaginationFilter(1, int.MaxValue);
    }
}