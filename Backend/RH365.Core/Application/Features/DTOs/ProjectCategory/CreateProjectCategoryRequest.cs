// ============================================================================
// Archivo: CreateProjectCategoryRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/DTOs/ProjectCategory/CreateProjectCategoryRequest.cs
// Descripción: DTO de entrada para crear una categoría de proyecto.
// ============================================================================
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.ProjectCategory
{
    /// <summary>Request para crear una categoría de proyecto.</summary>
    public class CreateProjectCategoryRequest
    {
        [Required, StringLength(100)]
        public string CategoryName { get; set; } = null!;

        [StringLength(50)]
        public string? LedgerAccount { get; set; }

        [Required]
        public long ProjectRefRecID { get; set; }

        public bool ProjectCategoryStatus { get; set; } = true;
    }
}
