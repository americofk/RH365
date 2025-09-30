// ============================================================================
// Archivo: UpdateDisabilityTypeRequest.cs
// Capa: RH365.Core.Application
// Ruta: Application/Features/DTOs/DisabilityType/UpdateDisabilityTypeRequest.cs
// Descripción: DTO de entrada para actualizar DisabilityTypes por RecID.
//   - Mantiene las mismas validaciones que Create.
// ============================================================================

using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.DisabilityType
{
    /// <summary>Payload para actualizar un tipo de discapacidad.</summary>
    public sealed class UpdateDisabilityTypeRequest
    {
        /// <summary>Código único (2..20). Se normaliza a MAYÚSCULAS.</summary>
        [Required, StringLength(20, MinimumLength = 2)]
        public string DisabilityTypeCode { get; set; } = null!;

        /// <summary>Descripción opcional (máx. 255).</summary>
        [StringLength(255)]
        public string? Description { get; set; }
    }
}
