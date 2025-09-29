// ============================================================================
// Archivo: UpdateDepartmentRequest.cs
// Proyecto: RH365.Core
// Ruta: Application/Features/DTOs/Department/UpdateDepartmentRequest.cs
// Descripción: DTO de entrada para actualizar un departamento existente.
// ============================================================================
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.Department
{
    /// <summary>Payload para actualizar un departamento existente.</summary>
    public sealed class UpdateDepartmentRequest
    {
        [Required, StringLength(10, MinimumLength = 2)]
        public string DepartmentCode { get; set; } = null!;

        [Required, StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = null!;
    }
}
