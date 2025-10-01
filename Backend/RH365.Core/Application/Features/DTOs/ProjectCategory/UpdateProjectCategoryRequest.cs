// ============================================================================
// Archivo: UpdateProjectCategoryRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/DTOs/ProjectCategory/UpdateProjectCategoryRequest.cs
// Descripción: DTO de entrada para actualizar una categoría de proyecto.
// ============================================================================
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.ProjectCategory
{
    /// <summary>Request para actualizar una categoría de proyecto.</summary>
    public class UpdateProjectCategoryRequest
    {
        [Required, StringLength(100)]
        public string CategoryName { get; set; } = null!;

        [StringLength(50)]
        public string? LedgerAccount { get; set; }

        [Required]
        public long ProjectRefRecID { get; set; }

        public bool ProjectCategoryStatus { get; set; }
    }
}
