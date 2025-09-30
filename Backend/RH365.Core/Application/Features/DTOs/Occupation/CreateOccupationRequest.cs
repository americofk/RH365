// ============================================================================
// Archivo: CreateOccupationRequest.cs
// Capa: RH365.Core.Application
// Ruta: Application/Features/DTOs/Occupation/CreateOccupationRequest.cs
// Descripción: DTO de entrada para crear Occupations.
//   - Validaciones por DataAnnotations.
//   - Normalización (Trim/ToUpper) en Controller.
// ============================================================================
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.Occupation
{
    /// <summary>Payload para crear una ocupación.</summary>
    public sealed class CreateOccupationRequest
    {
        [Required, StringLength(20, MinimumLength = 2)]
        public string OccupationCode { get; set; } = null!;

        [StringLength(255)]
        public string? Description { get; set; }
    }
}
