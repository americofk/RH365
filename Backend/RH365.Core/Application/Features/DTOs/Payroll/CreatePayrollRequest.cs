// ============================================================================
// Archivo: CreatePayrollRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/Payroll/CreatePayrollRequest.cs
// Descripción:
//   - DTO de creación para Payroll (dbo.Payrolls)
//   - No incluye ID/RecID (se generan en BD); auditoría la maneja el DbContext
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.Payroll
{
    /// <summary>
    /// Payload para crear una nómina.
    /// </summary>
    public class CreatePayrollRequest
    {
        /// <summary>Nombre de la nómina.</summary>
        public string Name { get; set; } = null!;

        /// <summary>Frecuencia de pago.</summary>
        public int PayFrecuency { get; set; }

        /// <summary>Vigente desde.</summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>Vigente hasta.</summary>
        public DateTime ValidTo { get; set; }

        /// <summary>Descripción (opcional).</summary>
        public string? Description { get; set; }

        /// <summary>Es nómina de regalía.</summary>
        public bool IsRoyaltyPayroll { get; set; }

        /// <summary>Es nómina por horas.</summary>
        public bool IsForHourPayroll { get; set; }

        /// <summary>Secuencia bancaria.</summary>
        public int BankSecuence { get; set; }

        /// <summary>FK a la divisa.</summary>
        public long CurrencyRefRecID { get; set; }

        /// <summary>Estado de la nómina.</summary>
        public bool PayrollStatus { get; set; } = true;

        /// <summary>Observaciones (opcional).</summary>
        public string? Observations { get; set; }
    }
}