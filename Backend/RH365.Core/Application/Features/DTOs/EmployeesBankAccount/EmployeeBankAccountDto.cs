// ============================================================================
// Archivo: EmployeeBankAccountDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/DTOs/EmployeesBankAccount/EmployeeBankAccountDto.cs
// Descripción: DTO de salida para cuentas bancarias de empleados.
// ============================================================================

using System;

namespace RH365.Core.Application.Features.DTOs.EmployeesBankAccount
{
    /// <summary>DTO de lectura para cuentas bancarias de un empleado.</summary>
    public class EmployeeBankAccountDto
    {
        // Identificadores
        public long RecID { get; set; }
        public string ID { get; set; } = null!;   // Ej: EBA-00000001

        // Datos de negocio
        public long EmployeeRefRecID { get; set; }
        public string BankName { get; set; } = null!;
        public int AccountType { get; set; }
        public string AccountNum { get; set; } = null!;
        public string? Currency { get; set; }
        public bool IsPrincipal { get; set; }
        public string? Comment { get; set; }

        // Auditoría / multiempresa
        public string DataareaID { get; set; } = null!;
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
