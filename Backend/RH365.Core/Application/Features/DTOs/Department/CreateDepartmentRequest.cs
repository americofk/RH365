// ============================================================================
// Archivo: CreateDepartmentRequest.cs
// Proyecto: RH365.Core
// Ruta: Application/Features/DTOs/Department/CreateDepartmentRequest.cs
// Descripción: DTO de entrada para crear un departamento.
// ============================================================================
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.Department
{
    /// <summary>Payload para crear un nuevo departamento.</summary>
    public sealed class CreateDepartmentRequest
    {
        [Required, StringLength(10, MinimumLength = 2)]
        public string DepartmentCode { get; set; } = null!;

        [Required, StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = null!;
    }
}
