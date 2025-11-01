// ============================================================================
// Archivo: UpdateJobRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Job/UpdateJobRequest.cs
// Descripción: Request para actualizar un cargo
// Estándar: ISO 27001 - Control de cambios en activos
// ============================================================================

using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Job
{
    public class UpdateJobRequest
    {
        [JsonPropertyName("JobCode")]
        public string JobCode { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("JobStatus")]
        public bool? JobStatus { get; set; }
    }
}
