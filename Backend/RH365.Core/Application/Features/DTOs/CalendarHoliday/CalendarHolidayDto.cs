// ============================================================================
// Archivo: CalendarHolidayDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/CalendarHoliday/CalendarHolidayDto.cs
// Descripción:
//   - DTO de lectura para la entidad CalendarHoliday (dbo.CalendarHolidays)
//   - Incluye claves, datos y auditoría (ISO 27001)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.CalendarHoliday
{
    /// <summary>
    /// DTO de salida para CalendarHoliday.
    /// </summary>
    public class CalendarHolidayDto
    {
        // Identificadores
        public long RecID { get; set; }
        public string? ID { get; set; }

        // Datos de negocio
        public DateTime CalendarDate { get; set; }
        public string Description { get; set; } = null!;
        public string? Observations { get; set; }

        // Auditoría ISO 27001
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}