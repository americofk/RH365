// ============================================================================
// Archivo: EmployeeWorkCalendar.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Employees/EmployeeWorkCalendar.cs
// Descripción: Entidad que representa el calendario de trabajo asignado a un empleado.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Permite registrar los horarios diarios de trabajo y descansos
// ============================================================================

using RH365.Core.Domain.Common;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa el calendario laboral de un empleado en un día específico.
    /// </summary>
    public class EmployeeWorkCalendar : AuditableCompanyEntity
    {
        /// <summary>
        /// FK al empleado relacionado.
        /// </summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// Fecha correspondiente al día laboral.
        /// </summary>
        public DateTime CalendarDate { get; set; }

        /// <summary>
        /// Nombre del día (ejemplo: Lunes, Martes).
        /// </summary>
        public string CalendarDay { get; set; } = null!;

        /// <summary>
        /// Hora de inicio de la jornada laboral.
        /// </summary>
        public TimeOnly WorkFrom { get; set; }

        /// <summary>
        /// Hora de finalización de la jornada laboral.
        /// </summary>
        public TimeOnly WorkTo { get; set; }

        /// <summary>
        /// Hora de inicio de la pausa de almuerzo o descanso.
        /// </summary>
        public TimeOnly BreakWorkFrom { get; set; }

        /// <summary>
        /// Hora de finalización de la pausa de almuerzo o descanso.
        /// </summary>
        public TimeOnly BreakWorkTo { get; set; }

        /// <summary>
        /// Total de horas laborales calculadas en el día.
        /// </summary>
        public decimal TotalHour { get; set; }

        /// <summary>
        /// Estado del control laboral en el día.
        /// </summary>
        public int StatusWorkControl { get; set; }

        /// <summary>
        /// FK al proceso de nómina en el que se registra (si aplica).
        /// </summary>
        public long? PayrollProcessRefRecID { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Empleado relacionado con el calendario laboral.
        /// </summary>
        public virtual Employee EmployeeRefRec { get; set; } = null!;
    }
}
