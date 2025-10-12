// ============================================================================
// Archivo: UpdateEmployeeWorkControlCalendarRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeWorkControlCalendar/UpdateEmployeeWorkControlCalendarRequest.cs
// Descripción:
//   - DTO de actualización parcial para EmployeeWorkControlCalendar
//   - Todos los campos son opcionales; solo se actualiza lo enviado
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeWorkControlCalendar
{
    /// <summary>
    /// Payload para actualizar (parcial) un registro de control de asistencia.
    /// </summary>
    public class UpdateEmployeeWorkControlCalendarRequest
    {
        public long? EmployeeRefRecID { get; set; }
        public DateTime? CalendarDate { get; set; }
        public string? CalendarDay { get; set; }
        public TimeOnly? WorkFrom { get; set; }
        public TimeOnly? WorkTo { get; set; }
        public TimeOnly? BreakWorkFrom { get; set; }
        public TimeOnly? BreakWorkTo { get; set; }
        public decimal? TotalHour { get; set; }
        public int? StatusWorkControl { get; set; }
        public long? PayrollProcessRefRecID { get; set; }
        public string? Observations { get; set; }
    }
}