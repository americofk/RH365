// ============================================================================
// Archivo: UpdateUserGridViewRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/UserGridViews/UpdateUserGridViewRequest.cs
// Descripción: Request para actualizar una vista existente
// ============================================================================

using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.UserGridViews
{
    public class UpdateUserGridViewRequest
    {
        [JsonPropertyName("ViewName")]
        public string ViewName { get; set; }

        [JsonPropertyName("ViewConfig")]
        public string ViewConfig { get; set; }

        [JsonPropertyName("IsDefault")]
        public bool? IsDefault { get; set; }
    }
}