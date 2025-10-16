// ============================================================================
// Archivo: ViewConfig.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/UserGridViews/ViewConfig.cs
// Descripción: Estructura de configuración de una vista
// ============================================================================

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.UserGridViews
{
    /// <summary>
    /// Configuración de columnas de una vista
    /// </summary>
    public class ColumnConfig
    {
        [JsonPropertyName("field")]
        public string Field { get; set; }

        [JsonPropertyName("visible")]
        public bool Visible { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("width")]
        public int? Width { get; set; }

        [JsonPropertyName("pinned")]
        public bool? Pinned { get; set; }
    }

    /// <summary>
    /// Configuración completa de la vista
    /// </summary>
    public class ViewConfig
    {
        [JsonPropertyName("columns")]
        public List<ColumnConfig> Columns { get; set; } = new();

        [JsonPropertyName("sorting")]
        public List<SortConfig> Sorting { get; set; } = new();

        [JsonPropertyName("filters")]
        public Dictionary<string, object> Filters { get; set; } = new();

        [JsonPropertyName("pageSize")]
        public int? PageSize { get; set; }
    }

    /// <summary>
    /// Configuración de ordenamiento
    /// </summary>
    public class SortConfig
    {
        [JsonPropertyName("field")]
        public string Field { get; set; }

        [JsonPropertyName("direction")]
        public string Direction { get; set; } // "asc" | "desc"
    }
}