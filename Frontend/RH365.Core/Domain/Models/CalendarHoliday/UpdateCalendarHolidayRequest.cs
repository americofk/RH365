// ============================================================================
// Archivo: UpdateCalendarHolidayRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/CalendarHoliday/UpdateCalendarHolidayRequest.cs
// Descripción: Request para actualizar un día feriado
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.CalendarHoliday
{
    public class UpdateCalendarHolidayRequest
    {
        [JsonPropertyName("CalendarDate")]
        public DateTime CalendarDate { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("Observations")]
        public string Observations { get; set; }
    }
}
