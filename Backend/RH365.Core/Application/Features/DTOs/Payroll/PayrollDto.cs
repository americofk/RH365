// ============================================================================
// Archivo: PayrollDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/Payroll/PayrollDto.cs
// Descripción:
//   - DTO de lectura para la entidad Payroll (dbo.Payrolls)
//   - Incluye claves, datos y auditoría (ISO 27001)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.Payroll
{
    /// <summary>
    /// DTO de salida para Payroll.
    /// </summary>
    public class PayrollDto
    {
        // Identificadores
        public long RecID { get; set; }
        public string? ID { get; set; }

        // Datos de negocio
        public string Name { get; set; } = null!;
        public int PayFrecuency { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string? Description { get; set; }
        public bool IsRoyaltyPayroll { get; set; }
        public bool IsForHourPayroll { get; set; }
        public int BankSecuence { get; set; }
        public long CurrencyRefRecID { get; set; }
        public bool PayrollStatus { get; set; }
        public string? Observations { get; set; }

        // Auditoría ISO 27001
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}