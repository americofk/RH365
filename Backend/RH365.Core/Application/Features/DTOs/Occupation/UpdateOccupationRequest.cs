// ============================================================================
// Archivo: UpdateOccupationRequest.cs
// Capa: RH365.Core.Application
// Ruta: Application/Features/DTOs/Occupation/UpdateOccupationRequest.cs
// Descripción: DTO de entrada para actualizar Occupations por RecID.
// ============================================================================
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.Occupation
{
    /// <summary>Payload para actualizar una ocupación.</summary>
    public sealed class UpdateOccupationRequest
    {
        [Required, StringLength(20, MinimumLength = 2)]
        public string OccupationCode { get; set; } = null!;

        [StringLength(255)]
        public string? Description { get; set; }
    }
}
