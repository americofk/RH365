// ============================================================================
// Archivo: CreateProjectRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/DTOs/Project/CreateProjectRequest.cs
// Descripción: DTO de entrada para crear un proyecto.
//   - Valida formato/longitud de campos.
// ============================================================================
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.Project
{
    /// <summary>Request para crear un proyecto.</summary>
    public class CreateProjectRequest
    {
        [Required, StringLength(40)]
        public string ProjectCode { get; set; } = null!;

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(50)]
        public string? LedgerAccount { get; set; }

        public bool ProjectStatus { get; set; } = true;
    }
}
