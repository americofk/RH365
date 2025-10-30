// ============================================================================
// Archivo: CreateOccupationRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Occupation/CreateOccupationRequest.cs
// Descripción: Request para crear una ocupación
// ============================================================================

using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Occupation
{
    public class CreateOccupationRequest
    {
        [JsonPropertyName("OccupationCode")]
        public string OccupationCode { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }
    }
}
