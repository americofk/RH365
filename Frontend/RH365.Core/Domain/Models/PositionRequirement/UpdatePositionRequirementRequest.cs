// ============================================================================
// Archivo: UpdatePositionRequirementRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/PositionRequirement/UpdatePositionRequirementRequest.cs
// Descripción: Request para actualizar un requisito de posición
// Estándar: ISO 27001 - Control A.12.1.1 (Procedimientos operacionales documentados)
// ============================================================================

using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.PositionRequirement
{
    public class UpdatePositionRequirementRequest
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Detail")]
        public string Detail { get; set; }

        [JsonPropertyName("PositionRefRecID")]
        public long? PositionRefRecID { get; set; }

        [JsonPropertyName("Observations")]
        public string Observations { get; set; }
    }
}