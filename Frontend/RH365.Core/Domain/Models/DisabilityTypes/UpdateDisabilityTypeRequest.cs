// ============================================================================
// Archivo: UpdateDisabilityTypeRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/DisabilityType/UpdateDisabilityTypeRequest.cs
// Descripción: Request para actualizar un tipo de discapacidad
// ============================================================================

using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.DisabilityType
{
    public class UpdateDisabilityTypeRequest
    {
        [JsonPropertyName("DisabilityTypeCode")]
        public string DisabilityTypeCode { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }
    }
}
