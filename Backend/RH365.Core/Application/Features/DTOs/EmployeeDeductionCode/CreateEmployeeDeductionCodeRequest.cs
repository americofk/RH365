// ============================================================================
// Archivo: CreateEmployeeDeductionCodeRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeDeductionCode/CreateEmployeeDeductionCodeRequest.cs
// Descripción:
//   - DTO de creación para EmployeeDeductionCode
//   - No incluye ID/RecID (se generan en BD)
//   - Auditoría la maneja el DbContext automáticamente
//   - Incluye todos los campos necesarios para configurar una deducción
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeDeductionCode
{
    /// <summary>
    /// Payload para crear una deducción de empleado.
    /// </summary>
    public class CreateEmployeeDeductionCodeRequest
    {
        /// <summary>FK al código de deducción.</summary>
        public long DeductionCodeRefRecID { get; set; }

        /// <summary>FK al empleado afectado por la deducción.</summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>FK a la nómina en la que se aplica la deducción.</summary>
        public long PayrollRefRecID { get; set; }

        /// <summary>Fecha de inicio de aplicación de la deducción.</summary>
        public DateTime FromDate { get; set; }

        /// <summary>Fecha de finalización de aplicación de la deducción.</summary>
        public DateTime ToDate { get; set; }

        /// <summary>Índice de deducción aplicado.</summary>
        public decimal IndexDeduction { get; set; }

        /// <summary>Porcentaje de deducción aplicado al empleado (0-100).</summary>
        public decimal PercentDeduction { get; set; }

        /// <summary>Porcentaje de contribución asociado (0-100).</summary>
        public decimal PercentContribution { get; set; }

        /// <summary>Comentario adicional sobre la deducción.</summary>
        public string? Comment { get; set; }

        /// <summary>Monto fijo de la deducción.</summary>
        public decimal DeductionAmount { get; set; }

        /// <summary>Frecuencia de pago (1=Semanal, 2=Quincenal, 3=Mensual, etc.).</summary>
        public int PayFrecuency { get; set; }

        /// <summary>Cantidad de períodos a pagar.</summary>
        public int QtyPeriodForPaid { get; set; }

        /// <summary>Período inicial de pago.</summary>
        public int StartPeriodForPaid { get; set; }

        /// <summary>Observaciones adicionales.</summary>
        public string? Observations { get; set; }
    }
}