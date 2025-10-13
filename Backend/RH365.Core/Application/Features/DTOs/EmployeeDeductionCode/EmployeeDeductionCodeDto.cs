// ============================================================================
// Archivo: EmployeeDeductionCodeDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeDeductionCode/EmployeeDeductionCodeDto.cs
// Descripción:
//   - DTO de lectura para la entidad EmployeeDeductionCode
//   - Incluye todos los campos de deducciones y cálculos
//   - Incluye claves, datos y auditoría (ISO 27001)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeDeductionCode
{
    /// <summary>
    /// DTO de salida para EmployeeDeductionCode.
    /// </summary>
    public class EmployeeDeductionCodeDto
    {
        /// <summary>Clave primaria del registro.</summary>
        public long RecID { get; set; }

        /// <summary>Identificador legible generado por secuencia.</summary>
        public string? ID { get; set; }

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

        /// <summary>Porcentaje de deducción aplicado al empleado.</summary>
        public decimal PercentDeduction { get; set; }

        /// <summary>Porcentaje de contribución asociado.</summary>
        public decimal PercentContribution { get; set; }

        /// <summary>Comentario adicional sobre la deducción.</summary>
        public string? Comment { get; set; }

        /// <summary>Monto fijo de la deducción.</summary>
        public decimal DeductionAmount { get; set; }

        /// <summary>Frecuencia de pago en la que aplica la deducción.</summary>
        public int PayFrecuency { get; set; }

        /// <summary>Cantidad de períodos a pagar.</summary>
        public int QtyPeriodForPaid { get; set; }

        /// <summary>Período inicial de pago.</summary>
        public int StartPeriodForPaid { get; set; }

        /// <summary>Observaciones adicionales.</summary>
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