// ============================================================================
// Archivo: CreateEmployeeBankAccountRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/DTOs/EmployeesBankAccount/CreateEmployeeBankAccountRequest.cs
// Descripción: DTO de entrada para crear cuentas bancarias de empleados.
// ============================================================================

using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.EmployeesBankAccount
{
    /// <summary>Request para crear una cuenta bancaria de empleado.</summary>
    public class CreateEmployeeBankAccountRequest
    {
        [Required]
        public long EmployeeRefRecID { get; set; }

        [Required, StringLength(80)]
        public string BankName { get; set; } = null!;

        [Required]
        public int AccountType { get; set; } // Enum en consumer

        [Required, StringLength(50, MinimumLength = 4)]
        public string AccountNum { get; set; } = null!;

        [StringLength(10)]
        public string? Currency { get; set; }  // Ej: DOP, USD

        public bool IsPrincipal { get; set; } = false;

        [StringLength(255)]
        public string? Comment { get; set; }
    }
}
