// ============================================================================
// Archivo: CreateDisabilityTypeRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/DisabilityType/CreateDisabilityTypeRequest.cs
// Descripción: Request para crear un tipo de discapacidad
// ============================================================================

using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.DisabilityType
{
    public class CreateDisabilityTypeRequest
    {
        [JsonPropertyName("DisabilityTypeCode")]
        public string DisabilityTypeCode { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }
    }
}
