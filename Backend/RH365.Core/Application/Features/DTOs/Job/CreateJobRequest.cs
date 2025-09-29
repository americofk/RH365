// ============================================================================
// Archivo: CreateJobRequest.cs
// Proyecto: RH365.Core
// Ruta: Application/Features/DTOs/Job/CreateJobRequest.cs
// Descripción: DTO de entrada para crear un Job.
// ============================================================================
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.Job
{
    /// <summary>Payload para crear un nuevo puesto (Job).</summary>
    public sealed class CreateJobRequest
    {
        [Required, StringLength(10, MinimumLength = 2)]
        public string JobCode { get; set; } = null!;

        [Required, StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }

        public bool JobStatus { get; set; } = true;
    }
}
