// ============================================================================
// Archivo: EmployeeWorkControlCalendar.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Employees/EmployeeWorkControlCalendar.cs
// Descripción: Entidad que representa el control de asistencia de un empleado en un día.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Permite registrar la jornada real trabajada vs. la planificada
// ============================================================================

using RH365.Core.Domain.Common;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa el control de asistencia y jornada laboral de un empleado en un día específico.
    /// </summary>
    public class EmployeeWorkControlCalendar : AuditableCompanyEntity
    {
        /// <summary>
        /// FK al empleado relacionado.
        /// </summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// Fecha correspondiente al registro de asistencia.
        /// </summary>
        public DateTime CalendarDate { get; set; }

        /// <summary>
        /// Nombre del día (ejemplo: Lunes, Martes).
        /// </summary>
        public string CalendarDay { get; set; } = null!;

        /// <summary>
        /// Hora real de inicio de la jornada laboral.
        /// </summary>
        public TimeOnly WorkFrom { get; set; }

        /// <summary>
        /// Hora real de finalización de la jornada laboral.
        /// </summary>
        public TimeOnly WorkTo { get; set; }

        /// <summary>
        /// Hora real de inicio de la pausa de descanso.
        /// </summary>
        public TimeOnly BreakWorkFrom { get; set; }

        /// <summary>
        /// Hora real de finalización de la pausa de descanso.
        /// </summary>
        public TimeOnly BreakWorkTo { get; set; }

        /// <summary>
        /// Total de horas trabajadas en el día.
        /// </summary>
        public decimal TotalHour { get; set; }

        /// <summary>
        /// Estado del control de asistencia (ejemplo: completo, ausente, parcial).
        /// </summary>
        public int StatusWorkControl { get; set; }

        /// <summary>
        /// FK al proceso de nómina en el que se refleja la asistencia (si aplica).
        /// </summary>
        public long? PayrollProcessRefRecID { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Empleado relacionado con el registro de control.
        /// </summary>
        public virtual Employee EmployeeRefRec { get; set; } = null!;
    }
}
