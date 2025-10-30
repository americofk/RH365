// ============================================================================
// Archivo: OccupationResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Occupation/OccupationResponse.cs
// Descripción: Response para una ocupación individual
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Occupation
{
    public class OccupationResponse
    {
        [JsonPropertyName("RecID")]
        public long RecID { get; set; }

        [JsonPropertyName("OccupationCode")]
        public string OccupationCode { get; set; }

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
