// ============================================================================
// Archivo: UpdateEmployeeBankAccountRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/DTOs/EmployeesBankAccount/UpdateEmployeeBankAccountRequest.cs
// Descripción: DTO de entrada para actualizar cuentas bancarias de empleados.
// ============================================================================

using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.EmployeesBankAccount
{
    /// <summary>Request para actualizar una cuenta bancaria de empleado.</summary>
    public class UpdateEmployeeBankAccountRequest
    {
        [Required]
        public long EmployeeRefRecID { get; set; }

        [Required, StringLength(80)]
        public string BankName { get; set; } = null!;

        [Required]
        public int AccountType { get; set; }

        [Required, StringLength(50, MinimumLength = 4)]
        public string AccountNum { get; set; } = null!;

        [StringLength(10)]
        public string? Currency { get; set; }

        public bool IsPrincipal { get; set; }

        [StringLength(255)]
        public string? Comment { get; set; }
    }
}
