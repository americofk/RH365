// ============================================================================
// Archivo: CalendarHoliday.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Calendar/CalendarHoliday.cs
// Descripción: Entidad que representa los días festivos definidos en el calendario.
//   - Incluye herencia de AuditableCompanyEntity para cumplir con estándares ISO 27001
//   - Permite gestionar feriados nacionales/empresariales en el sistema multiempresa
// ============================================================================

using RH365.Core.Domain.Common;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un día festivo dentro del calendario de la empresa.
    /// </summary>
    public class CalendarHoliday : AuditableCompanyEntity
    {
        /// <summary>
        /// Fecha del día festivo.
        /// </summary>
        public DateTime CalendarDate { get; set; }

        /// <summary>
        /// Descripción del día festivo (ej. "Día de la Independencia").
        /// </summary>
        public string Description { get; set; } = null!;
    }
}
