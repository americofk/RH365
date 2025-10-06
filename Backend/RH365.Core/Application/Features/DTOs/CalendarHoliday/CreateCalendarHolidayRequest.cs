// ============================================================================
// Archivo: CreateCalendarHolidayRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/CalendarHoliday/CreateCalendarHolidayRequest.cs
// Descripción:
//   - DTO de creación para CalendarHoliday (dbo.CalendarHolidays)
//   - No incluye ID/RecID (se generan en BD); auditoría la maneja el DbContext
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.CalendarHoliday
{
    /// <summary>
    /// Payload para crear un día festivo.
    /// </summary>
    public class CreateCalendarHolidayRequest
    {
        /// <summary>Fecha del día festivo.</summary>
        public DateTime CalendarDate { get; set; }

        /// <summary>Descripción del día festivo.</summary>
        public string Description { get; set; } = null!;

        /// <summary>Observaciones (opcional).</summary>
        public string? Observations { get; set; }
    }
}