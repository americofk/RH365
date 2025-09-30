// ============================================================================
// Archivo: EmployeesAddressDto.cs
// Capa: RH365.Core.Application
// Ruta: Application/Features/DTOs/EmployeesAddress/EmployeesAddressDto.cs
// Descripción: DTO de salida (lectura) para direcciones de empleados.
//   - Incluye auditoría (ISO 27001).
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeesAddress
{
    /// <summary>DTO de lectura para EmployeesAddress.</summary>
    public sealed class EmployeesAddressDto
    {
        public long RecID { get; set; }
        public long EmployeeRefRecID { get; set; }
        public long CountryRefRecID { get; set; }

        public string Street { get; set; } = null!;
        public string Home { get; set; } = null!;
        public string Sector { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Province { get; set; } = null!;
        public string? ProvinceName { get; set; }
        public string? Comment { get; set; }
        public bool IsPrincipal { get; set; }

        // Auditoría
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
