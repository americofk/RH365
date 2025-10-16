// ============================================================================
// Archivo: SaveUserGridViewRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/UserGridViews/SaveUserGridViewRequest.cs
// Descripción: Request para guardar una vista nueva
// ============================================================================

using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.UserGridViews
{
    public class SaveUserGridViewRequest
    {
        [JsonPropertyName("EntityName")]
        public string EntityName { get; set; }

        [JsonPropertyName("ViewName")]
        public string ViewName { get; set; }

        [JsonPropertyName("ViewConfig")]
        public string ViewConfig { get; set; }

        [JsonPropertyName("IsDefault")]
        public bool IsDefault { get; set; }

        [JsonPropertyName("IsPublic")]
        public bool IsPublic { get; set; }

        [JsonPropertyName("ViewScope")]
        public string ViewScope { get; set; } = "Private";
    }
}