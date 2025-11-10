// ============================================================================
// Archivo: CreatePositionRequirementRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/PositionRequirement/CreatePositionRequirementRequest.cs
// Descripción: Request para crear un requisito de posición
// Estándar: ISO 27001 - Control A.12.1.1 (Procedimientos operacionales documentados)
// ============================================================================

using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.PositionRequirement
{
    public class CreatePositionRequirementRequest
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Detail")]
        public string Detail { get; set; }

        [JsonPropertyName("PositionRefRecID")]
        public long PositionRefRecID { get; set; }

        [JsonPropertyName("Observations")]
        public string Observations { get; set; }
    }
}