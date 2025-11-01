// ============================================================================
// Archivo: JobResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Job/JobResponse.cs
// Descripción: Response para un cargo individual
// Estándar: ISO 27001 - Trazabilidad de información
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Job
{
    public class JobResponse
    {
        [JsonPropertyName("RecID")]
        public long RecID { get; set; }

        [JsonPropertyName("JobCode")]
        public string JobCode { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("JobStatus")]
        public bool JobStatus { get; set; }

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
