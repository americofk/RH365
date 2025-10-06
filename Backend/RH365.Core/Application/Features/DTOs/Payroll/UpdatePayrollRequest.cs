// ============================================================================
// Archivo: UpdatePayrollRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/Payroll/UpdatePayrollRequest.cs
// Descripción:
//   - DTO de actualización parcial para Payroll (dbo.Payrolls)
//   - Todos los campos son opcionales; solo se actualiza lo enviado
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.Payroll
{
    /// <summary>
    /// Payload para actualizar (parcial) una nómina.
    /// </summary>
    public class UpdatePayrollRequest
    {
        public string? Name { get; set; }
        public int? PayFrecuency { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string? Description { get; set; }
        public bool? IsRoyaltyPayroll { get; set; }
        public bool? IsForHourPayroll { get; set; }
        public int? BankSecuence { get; set; }
        public long? CurrencyRefRecID { get; set; }
        public bool? PayrollStatus { get; set; }
        public string? Observations { get; set; }
    }
}