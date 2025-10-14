// ============================================================================
// Archivo: MenuItemResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Models/Menu/MenuItemResponse.cs
// Descripción:
//   - Modelo para respuesta de menús del API
//   - Mantiene consistencia con LoginResponse
// ============================================================================

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Menu
{
    /// <summary>
    /// Response para items del menú
    /// </summary>
    public class MenuItemResponse
    {
        [JsonPropertyName("recID")]
        public long RecID { get; set; }

        [JsonPropertyName("id")]
        public string ID { get; set; } = null!;

        [JsonPropertyName("menuCode")]
        public string MenuCode { get; set; } = null!;

        [JsonPropertyName("menuName")]
        public string MenuName { get; set; } = null!;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("action")]
        public string? Action { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; } = null!;

        [JsonPropertyName("menuFatherRefRecID")]
        public long? MenuFatherRefRecID { get; set; }

        [JsonPropertyName("sort")]
        public int Sort { get; set; }

        [JsonPropertyName("isViewMenu")]
        public bool IsViewMenu { get; set; }

        [JsonPropertyName("dataAreaId")]
        public string DataAreaId { get; set; } = null!;

        /// <summary>
        /// Hijos del menú (se llena en el servicio)
        /// </summary>
        public List<MenuItemResponse> Children { get; set; } = new();

        /// <summary>
        /// Indica si tiene hijos
        /// </summary>
        public bool HasChildren => Children != null && Children.Count > 0;
    }
}