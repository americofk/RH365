// ============================================================================
// Archivo: UpdateEmployeesAddressRequest.cs
// Capa: RH365.Core.Application
// Ruta: Application/Features/DTOs/EmployeesAddress/UpdateEmployeesAddressRequest.cs
// Descripción: DTO de entrada para actualizar direcciones de empleados.
// ============================================================================
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.EmployeesAddress
{
    /// <summary>Payload para actualizar una dirección de empleado.</summary>
    public sealed class UpdateEmployeesAddressRequest
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

        public bool IsPrincipal { get; set; }

        [Required]
        public long CountryRefRecID { get; set; }
    }
}
