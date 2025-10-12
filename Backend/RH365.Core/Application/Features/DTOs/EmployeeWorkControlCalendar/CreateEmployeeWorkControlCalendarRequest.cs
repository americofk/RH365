// ============================================================================
// Archivo: CreateEmployeeWorkControlCalendarRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeWorkControlCalendar/CreateEmployeeWorkControlCalendarRequest.cs
// Descripción:
//   - DTO de creación para EmployeeWorkControlCalendar
//   - No incluye ID/RecID (se generan en BD); auditoría la maneja el DbContext
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeWorkControlCalendar
{
    /// <summary>
    /// Payload para crear un registro de control de asistencia.
    /// </summary>
    public class CreateEmployeeWorkControlCalendarRequest
    {
        public long EmployeeRefRecID { get; set; }
        public DateTime CalendarDate { get; set; }
        public string CalendarDay { get; set; } = null!;
        public TimeOnly WorkFrom { get; set; }
        public TimeOnly WorkTo { get; set; }
        public TimeOnly BreakWorkFrom { get; set; }
        public TimeOnly BreakWorkTo { get; set; }
        public decimal TotalHour { get; set; }
        public int StatusWorkControl { get; set; }
        public long? PayrollProcessRefRecID { get; set; }
        public string? Observations { get; set; }
    }
}