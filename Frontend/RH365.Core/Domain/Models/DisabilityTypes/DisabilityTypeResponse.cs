// ============================================================================
// Archivo: DisabilityTypeResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/DisabilityType/DisabilityTypeResponse.cs
// Descripción: Response para un tipo de discapacidad individual
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.DisabilityType
{
    public class DisabilityTypeResponse
    {
        [JsonPropertyName("RecID")]
        public long RecID { get; set; }

        [JsonPropertyName("DisabilityTypeCode")]
        public string DisabilityTypeCode { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("CreatedBy")]
        public string CreatedBy { get; set; }

        [JsonPropertyName("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [JsonPropertyName("ModifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonPropertyName("ModifiedOn")]
        public DateTime? ModifiedOn { get; set; }
    }
}
