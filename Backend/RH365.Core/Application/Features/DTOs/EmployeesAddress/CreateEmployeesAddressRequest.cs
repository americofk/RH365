// ============================================================================
// Archivo: CreateEmployeesAddressRequest.cs
// Capa: RH365.Core.Application
// Ruta: Application/Features/DTOs/EmployeesAddress/CreateEmployeesAddressRequest.cs
// Descripción: DTO de entrada para crear direcciones de empleados.
//   - Validaciones vía DataAnnotations.
//   - Normalización (Trim/Upper) se realiza en el Controller.
// ============================================================================
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.EmployeesAddress
{
    /// <summary>Payload para crear una dirección de empleado.</summary>
    public sealed class CreateEmployeesAddressRequest
    {
        [Required]
        public long EmployeeRefRecID { get; set; }

        [Required, StringLength(150, MinimumLength = 1)]
        public string Street { get; set; } = null!;

        [Required, StringLength(30, MinimumLength = 1)]
        public string Home { get; set; } = null!;

        [Required, StringLength(100, MinimumLength = 1)]
        public string Sector { get; set; } = null!;

        [Required, StringLength(100, MinimumLength = 1)]
        public string City { get; set; } = null!;

        [Required, StringLength(100, MinimumLength = 1)]
        public string Province { get; set; } = null!;

        [StringLength(100)]
        public string? ProvinceName { get; set; }

        [StringLength(255)]
        public string? Comment { get; set; }

        /// <summary>Si es principal para el empleado (único por empleado).</summary>
        public bool IsPrincipal { get; set; } = false;

        [Required]
        public long CountryRefRecID { get; set; }
    }
}
