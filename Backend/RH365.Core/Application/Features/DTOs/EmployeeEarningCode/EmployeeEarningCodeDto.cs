// ============================================================================
// Archivo: EmployeeEarningCodeDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeEarningCode/EmployeeEarningCodeDto.cs
// Descripción:
//   - DTO de lectura para la entidad EmployeeEarningCode
//   - Incluye todos los campos de percepciones y cálculos
//   - Incluye claves, datos y auditoría (ISO 27001)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeEarningCode
{
    /// <summary>
    /// DTO de salida para EmployeeEarningCode.
    /// </summary>
    public class EmployeeEarningCodeDto
    {
        /// <summary>Clave primaria del registro.</summary>
        public long RecID { get; set; }

        /// <summary>Identificador legible generado por secuencia.</summary>
        public string? ID { get; set; }

        /// <summary>FK al código de percepción.</summary>
        public long EarningCodeRefRecID { get; set; }

        /// <summary>FK al empleado que recibe la percepción.</summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>FK a la nómina donde se aplica la percepción.</summary>
        public long PayrollRefRecID { get; set; }

        /// <summary>FK al proceso de nómina (opcional).</summary>
        public long? PayrollProcessRefRecID { get; set; }

        /// <summary>Fecha de inicio de aplicación.</summary>
        public DateTime FromDate { get; set; }

        /// <summary>Fecha de finalización de aplicación.</summary>
        public DateTime ToDate { get; set; }

        /// <summary>Índice de cálculo de la percepción.</summary>
        public decimal IndexEarning { get; set; }

        /// <summary>Cantidad de unidades (ejemplo: horas).</summary>
        public int Quantity { get; set; }

        /// <summary>Comentario adicional sobre la percepción.</summary>
        public string? Comment { get; set; }

        /// <summary>Cantidad de períodos de pago.</summary>
        public int QtyPeriodForPaid { get; set; }

        /// <summary>Período inicial de pago.</summary>
        public int StartPeriodForPaid { get; set; }

        /// <summary>Índice de percepción mensual.</summary>
        public decimal IndexEarningMonthly { get; set; }

        /// <summary>Frecuencia de pago.</summary>
        public int PayFrecuency { get; set; }

        /// <summary>Índice de percepción diaria.</summary>
        public decimal IndexEarningDiary { get; set; }

        /// <summary>Indica si se usa para reportes a la DGT.</summary>
        public bool IsUseDgt { get; set; }

        /// <summary>Índice de percepción por hora.</summary>
        public decimal IndexEarningHour { get; set; }

        /// <summary>Indica si se utiliza cálculo por hora.</summary>
        public bool IsUseCalcHour { get; set; }

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