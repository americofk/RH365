// ============================================================================
// Archivo: CreateJobRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Job/CreateJobRequest.cs
// Descripción: Request para crear un cargo
// Estándar: ISO 27001 - Gestión de activos de información
// ============================================================================

using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Job
{
    public class CreateJobRequest
    {
        [JsonPropertyName("JobCode")]
        public string JobCode { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("JobStatus")]
        public bool JobStatus { get; set; }
    }
}
