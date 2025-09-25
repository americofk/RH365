// ============================================================================
// Archivo: EmployeeExtraHour.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Employees/EmployeeExtraHour.cs
// Descripción: Entidad que representa las horas extras trabajadas por un empleado.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Incluye relación con códigos de percepción, empleado y nómina
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Core.Domain.Entities;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa las horas extras realizadas por un empleado.
    /// </summary>
    public class EmployeeExtraHour : AuditableCompanyEntity
    {
        /// <summary>
        /// FK al empleado que realizó las horas extras.
        /// </summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// FK al código de percepción asociado.
        /// </summary>
        public long EarningCodeRefRecID { get; set; }

        /// <summary>
        /// FK a la nómina en la que se registran las horas extras.
        /// </summary>
        public long PayrollRefRecID { get; set; }

        /// <summary>
        /// Día en el que se trabajaron las horas extras.
        /// </summary>
        public DateTime WorkedDay { get; set; }

        /// <summary>
        /// Hora de inicio de la jornada extra.
        /// </summary>
        public TimeOnly StartHour { get; set; }

        /// <summary>
        /// Hora de finalización de la jornada extra.
        /// </summary>
        public TimeOnly EndHour { get; set; }

        /// <summary>
        /// Monto total calculado por las horas extras.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Índice de cálculo aplicado a las horas extras.
        /// </summary>
        public decimal Indice { get; set; }

        /// <summary>
        /// Cantidad de horas trabajadas.
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Estado del registro de horas extras.
        /// </summary>
        public int StatusExtraHour { get; set; }

        /// <summary>
        /// Fecha en la que se calcula la nómina correspondiente.
        /// </summary>
        public DateTime CalcPayrollDate { get; set; }

        /// <summary>
        /// Comentario adicional sobre las horas extras.
        /// </summary>
        public string? Comment { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Código de percepción asociado.
        /// </summary>
        public virtual EarningCode EarningCodeRefRec { get; set; } = null!;

        /// <summary>
        /// Empleado que realizó las horas extras.
        /// </summary>
        public virtual Employee EmployeeRefRec { get; set; } = null!;

        /// <summary>
        /// Nómina en la que se registran las horas extras.
        /// </summary>
        public virtual Payroll PayrollRefRec { get; set; } = null!;
    }
}
