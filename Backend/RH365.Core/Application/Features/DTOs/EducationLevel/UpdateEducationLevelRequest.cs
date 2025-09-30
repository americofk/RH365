// ============================================================================
// Archivo: UpdateEducationLevelRequest.cs
// Capa: RH365.Core.Application
// Ruta: Application/Features/DTOs/EducationLevel/UpdateEducationLevelRequest.cs
// Descripción: DTO de entrada para actualizar EducationLevels por RecID.
// ============================================================================
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.EducationLevel
{
    /// <summary>Payload para actualizar un nivel educativo.</summary>
    public sealed class UpdateEducationLevelRequest
    {
        [Required, StringLength(20, MinimumLength = 2)]
        public string EducationLevelCode { get; set; } = null!;

        [StringLength(255)]
        public string? Description { get; set; }
    }
}
