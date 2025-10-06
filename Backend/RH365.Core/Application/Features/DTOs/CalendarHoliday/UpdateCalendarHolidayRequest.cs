// ============================================================================
// Archivo: UpdateCalendarHolidayRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/CalendarHoliday/UpdateCalendarHolidayRequest.cs
// Descripción:
//   - DTO de actualización parcial para CalendarHoliday (dbo.CalendarHolidays)
//   - Todos los campos son opcionales; solo se actualiza lo enviado
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.CalendarHoliday
{
    /// <summary>
    /// Payload para actualizar (parcial) un día festivo.
    /// </summary>
    public class UpdateCalendarHolidayRequest
    {
        public DateTime? CalendarDate { get; set; }
        public string? Description { get; set; }
        public string? Observations { get; set; }
    }
}