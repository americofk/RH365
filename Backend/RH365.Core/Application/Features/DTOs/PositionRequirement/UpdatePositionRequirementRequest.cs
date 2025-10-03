// ============================================================================
// Archivo: UpdatePositionRequirementRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/PositionRequirement/UpdatePositionRequirementRequest.cs
// Descripción:
//   - DTO de actualización parcial para PositionRequirement (dbo.PositionRequirements)
//   - Todos los campos son opcionales; solo se actualiza lo enviado
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.PositionRequirement
{
    /// <summary>
    /// Payload para actualizar (parcial) un requisito de puesto.
    /// </summary>
    public class UpdatePositionRequirementRequest
    {
        public string? Name { get; set; }
        public string? Detail { get; set; }
        public long? PositionRefRecID { get; set; }
        public string? Observations { get; set; }
    }
}