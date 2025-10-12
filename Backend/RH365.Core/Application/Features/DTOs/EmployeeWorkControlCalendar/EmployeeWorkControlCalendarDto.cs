// ============================================================================
// Archivo: EmployeeWorkControlCalendarDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeWorkControlCalendar/EmployeeWorkControlCalendarDto.cs
// Descripción:
//   - DTO de lectura para la entidad EmployeeWorkControlCalendar
//   - Incluye claves, datos y auditoría (ISO 27001)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeWorkControlCalendar
{
    /// <summary>
    /// DTO de salida para EmployeeWorkControlCalendar.
    /// </summary>
    public class EmployeeWorkControlCalendarDto
    {
        public long RecID { get; set; }
        public string? ID { get; set; }
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
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}