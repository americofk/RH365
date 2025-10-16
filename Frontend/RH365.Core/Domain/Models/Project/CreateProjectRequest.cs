// ============================================================================
// Archivo: CreateProjectRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Project/CreateProjectRequest.cs
// Descripción: Request para crear un proyecto
// ============================================================================

using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Project
{
    public class CreateProjectRequest
    {
        [JsonPropertyName("ProjectCode")]
        public string ProjectCode { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("LedgerAccount")]
        public string LedgerAccount { get; set; }

        [JsonPropertyName("ProjectStatus")]
        public bool ProjectStatus { get; set; }

        [JsonPropertyName("Observations")]
        public string Observations { get; set; }
    }
}