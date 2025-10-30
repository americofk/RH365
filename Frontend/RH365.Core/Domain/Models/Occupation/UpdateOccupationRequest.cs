// ============================================================================
// Archivo: UpdateOccupationRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Occupation/UpdateOccupationRequest.cs
// Descripción: Request para actualizar una ocupación
// ============================================================================

using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Occupation
{
    public class UpdateOccupationRequest
    {
        [JsonPropertyName("OccupationCode")]
        public string OccupationCode { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }
    }
}
