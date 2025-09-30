// ============================================================================
// Archivo: CreateEducationLevelRequest.cs
// Capa: RH365.Core.Application
// Ruta: Application/Features/DTOs/EducationLevel/CreateEducationLevelRequest.cs
// Descripción: DTO de entrada para crear EducationLevels.
//   - Validaciones por DataAnnotations.
//   - Normalización (Trim/ToUpper) en Controller.
// ============================================================================
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.EducationLevel
{
    /// <summary>Payload para crear un nivel educativo.</summary>
    public sealed class CreateEducationLevelRequest
    {
        [Required, StringLength(20, MinimumLength = 2)]
        public string EducationLevelCode { get; set; } = null!;

        [StringLength(255)]
        public string? Description { get; set; }
    }
}
