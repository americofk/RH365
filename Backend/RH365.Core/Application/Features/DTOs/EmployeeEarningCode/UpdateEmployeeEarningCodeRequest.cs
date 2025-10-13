// ============================================================================
// Archivo: UpdateEmployeeEarningCodeRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeEarningCode/UpdateEmployeeEarningCodeRequest.cs
// Descripción:
//   - DTO de actualización parcial para EmployeeEarningCode
//   - Todos los campos son opcionales
//   - Solo se actualiza lo enviado en el request
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeEarningCode
{
    /// <summary>
    /// Payload para actualizar (parcial) una percepción de empleado.
    /// </summary>
    public class UpdateEmployeeEarningCodeRequest
    {
        /// <summary>FK al código de percepción.</summary>
        public long? EarningCodeRefRecID { get; set; }

        /// <summary>FK al empleado que recibe la percepción.</summary>
        public long? EmployeeRefRecID { get; set; }

        /// <summary>FK a la nómina donde se aplica la percepción.</summary>
        public long? PayrollRefRecID { get; set; }

        /// <summary>FK al proceso de nómina (opcional).</summary>
        public long? PayrollProcessRefRecID { get; set; }

        /// <summary>Fecha de inicio de aplicación.</summary>
        public DateTime? FromDate { get; set; }

        /// <summary>Fecha de finalización de aplicación.</summary>
        public DateTime? ToDate { get; set; }

        /// <summary>Índice de cálculo de la percepción.</summary>
        public decimal? IndexEarning { get; set; }

        /// <summary>Cantidad de unidades (ejemplo: horas, días).</summary>
        public int? Quantity { get; set; }

        /// <summary>Comentario adicional sobre la percepción.</summary>
        public string? Comment { get; set; }

        /// <summary>Cantidad de períodos de pago.</summary>
        public int? QtyPeriodForPaid { get; set; }

        /// <summary>Período inicial de pago.</summary>
        public int? StartPeriodForPaid { get; set; }

        /// <summary>Índice de percepción mensual.</summary>
        public decimal? IndexEarningMonthly { get; set; }

        /// <summary>Frecuencia de pago.</summary>
        public int? PayFrecuency { get; set; }

        /// <summary>Índice de percepción diaria.</summary>
        public decimal? IndexEarningDiary { get; set; }

        /// <summary>Indica si se usa para reportes a la DGT.</summary>
        public bool? IsUseDgt { get; set; }

        /// <summary>Índice de percepción por hora.</summary>
        public decimal? IndexEarningHour { get; set; }

        /// <summary>Indica si se utiliza cálculo por hora.</summary>
        public bool? IsUseCalcHour { get; set; }

        /// <summary>Observaciones adicionales.</summary>
        public string? Observations { get; set; }
    }
}