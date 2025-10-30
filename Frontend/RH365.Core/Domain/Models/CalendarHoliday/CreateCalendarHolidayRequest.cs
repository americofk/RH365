// ============================================================================
// Archivo: CreateCalendarHolidayRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/CalendarHoliday/CreateCalendarHolidayRequest.cs
// Descripción: Request para crear un día feriado
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.CalendarHoliday
{
    public class CreateCalendarHolidayRequest
    {
        [JsonPropertyName("CalendarDate")]
        public DateTime CalendarDate { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("Observations")]
        public string Observations { get; set; }
    }
}
