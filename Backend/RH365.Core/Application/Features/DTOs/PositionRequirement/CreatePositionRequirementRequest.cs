// ============================================================================
// Archivo: CreatePositionRequirementRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/PositionRequirement/CreatePositionRequirementRequest.cs
// Descripción:
//   - DTO de creación para PositionRequirement (dbo.PositionRequirements)
//   - No incluye ID/RecID (se generan en BD); auditoría la maneja el DbContext
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.PositionRequirement
{
    /// <summary>
    /// Payload para crear un requisito de puesto.
    /// </summary>
    public class CreatePositionRequirementRequest
    {
        /// <summary>Nombre del requisito.</summary>
        public string Name { get; set; } = null!;

        /// <summary>Detalle o descripción del requisito.</summary>
        public string Detail { get; set; } = null!;

        /// <summary>FK al puesto.</summary>
        public long PositionRefRecID { get; set; }

        /// <summary>Observaciones (opcional).</summary>
        public string? Observations { get; set; }
    }
}