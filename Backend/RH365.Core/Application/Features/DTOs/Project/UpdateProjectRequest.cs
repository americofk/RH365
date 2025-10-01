// ============================================================================
// Archivo: UpdateProjectRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/DTOs/Project/UpdateProjectRequest.cs
// Descripción: DTO de entrada para actualizar un proyecto.
// ============================================================================
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.Project
{
    /// <summary>Request para actualizar un proyecto.</summary>
    public class UpdateProjectRequest
    {
        [Required, StringLength(40)]
        public string ProjectCode { get; set; } = null!;

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(50)]
        public string? LedgerAccount { get; set; }

        public bool ProjectStatus { get; set; }
    }
}
