// ============================================================================
// Archivo: SearchFilter.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Common/Filters/SearchFilter.cs
// Descripción: Filtro para búsquedas dinámicas en entidades.
//   - Búsqueda por propiedad y valor
//   - Soporta múltiples tipos de comparación
//   - Genera expresiones lambda dinámicamente
// ============================================================================

using System;
using System.Linq.Expressions;

namespace RH365.Core.Application.Common.Filters
{
    /// <summary>
    /// Filtro de búsqueda genérico para consultas.
    /// Permite buscar por cualquier propiedad de una entidad.
    /// </summary>
    public class SearchFilter
    {
        /// <summary>
        /// Nombre de la propiedad a buscar.
        /// Debe existir en la entidad.
        /// </summary>
        public string? PropertyName { get; set; }

        /// <summary>
        /// Valor a buscar en la propiedad.
        /// Se convierte al tipo apropiado.
        /// </summary>
        public string? PropertyValue { get; set; }

        /// <summary>
        /// Tipo de comparación a realizar.
        /// Por defecto: Contains para strings, Equals para otros.
        /// </summary>
        public SearchComparison Comparison { get; set; } = SearchComparison.Contains;

        /// <summary>
        /// Constructor vacío.
        /// </summary>
        public SearchFilter()
        {
        }

        /// <summary>
        /// Constructor con valores.
        /// </summary>
        /// <param name="propertyName">Propiedad donde buscar.</param>
        /// <param name="propertyValue">Valor a buscar.</param>
        public SearchFilter(string propertyName, string propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        /// <summary>
        /// Constructor completo con tipo de comparación.
        /// </summary>
        /// <param name="propertyName">Propiedad donde buscar.</param>
        /// <param name="propertyValue">Valor a buscar.</param>
        /// <param name="comparison">Tipo de comparación.</param>
        public SearchFilter(string propertyName, string propertyValue, SearchComparison comparison)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
            Comparison = comparison;
        }

        /// <summary>
        /// Valida si el filtro tiene datos válidos.
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(PropertyName) &&
                   !string.IsNullOrWhiteSpace(PropertyValue);
        }

        /// <summary>
        /// Limpia los valores del filtro.
        /// </summary>
        public void Clear()
        {
            PropertyName = null;
            PropertyValue = null;
            Comparison = SearchComparison.Contains;
        }
    }

    /// <summary>
    /// Filtro de búsqueda tipado para entidades específicas.
    /// Más seguro que la versión no tipada.
    /// </summary>
    /// <typeparam name="T">Tipo de la entidad a buscar.</typeparam>
    public class SearchFilter<T> : SearchFilter where T : class
    {
        /// <summary>
        /// Constructor vacío.
        /// </summary>
        public SearchFilter() : base()
        {
        }

        /// <summary>
        /// Constructor con valores.
        /// </summary>
        public SearchFilter(string propertyName, string propertyValue)
            : base(propertyName, propertyValue)
        {
        }

        /// <summary>
        /// Constructor completo.
        /// </summary>
        public SearchFilter(string propertyName, string propertyValue, SearchComparison comparison)
            : base(propertyName, propertyValue, comparison)
        {
        }

        /// <summary>
        /// Valida que la propiedad existe en el tipo T.
        /// </summary>
        public bool PropertyExists()
        {
            if (string.IsNullOrWhiteSpace(PropertyName))
                return false;

            var type = typeof(T);
            var property = type.GetProperty(PropertyName);
            return property != null;
        }

        /// <summary>
        /// Obtiene el tipo de la propiedad.
        /// </summary>
        public Type? GetPropertyType()
        {
            if (!PropertyExists())
                return null;

            var type = typeof(T);
            return type.GetProperty(PropertyName!)?.PropertyType;
        }
    }

    /// <summary>
    /// Tipos de comparación soportados en búsquedas.
    /// </summary>
    public enum SearchComparison
    {
        /// <summary>
        /// Igualdad exacta.
        /// </summary>
        Equals,

        /// <summary>
        /// Diferente de.
        /// </summary>
        NotEquals,

        /// <summary>
        /// Contiene el texto (solo strings).
        /// </summary>
        Contains,

        /// <summary>
        /// Empieza con (solo strings).
        /// </summary>
        StartsWith,

        /// <summary>
        /// Termina con (solo strings).
        /// </summary>
        EndsWith,

        /// <summary>
        /// Mayor que.
        /// </summary>
        GreaterThan,

        /// <summary>
        /// Mayor o igual que.
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        /// Menor que.
        /// </summary>
        LessThan,

        /// <summary>
        /// Menor o igual que.
        /// </summary>
        LessThanOrEqual,

        /// <summary>
        /// Valor está en un rango.
        /// </summary>
        Between,

        /// <summary>
        /// Es nulo o vacío.
        /// </summary>
        IsNullOrEmpty,

        /// <summary>
        /// No es nulo ni vacío.
        /// </summary>
        IsNotNullOrEmpty
    }

    /// <summary>
    /// Filtro de búsqueda avanzada con múltiples criterios.
    /// Permite combinar varios filtros con AND/OR.
    /// </summary>
    public class AdvancedSearchFilter
    {
        /// <summary>
        /// Lista de filtros a aplicar.
        /// </summary>
        public List<SearchFilter> Filters { get; set; } = new List<SearchFilter>();

        /// <summary>
        /// Operador lógico para combinar filtros.
        /// </summary>
        public LogicalOperator Operator { get; set; } = LogicalOperator.And;

        /// <summary>
        /// Agrega un nuevo filtro a la lista.
        /// </summary>
        /// <param name="filter">Filtro a agregar.</param>
        public void AddFilter(SearchFilter filter)
        {
            if (filter != null && filter.IsValid())
            {
                Filters.Add(filter);
            }
        }

        /// <summary>
        /// Limpia todos los filtros.
        /// </summary>
        public void Clear()
        {
            Filters.Clear();
        }

        /// <summary>
        /// Verifica si hay filtros válidos.
        /// </summary>
        public bool HasFilters => Filters.Count > 0;
    }

    /// <summary>
    /// Operadores lógicos para combinar filtros.
    /// </summary>
    public enum LogicalOperator
    {
        /// <summary>
        /// Todos los filtros deben cumplirse.
        /// </summary>
        And,

        /// <summary>
        /// Al menos un filtro debe cumplirse.
        /// </summary>
        Or
    }
}