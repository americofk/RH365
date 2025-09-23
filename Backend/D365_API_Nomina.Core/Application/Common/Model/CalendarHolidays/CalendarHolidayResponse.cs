using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.CalendarHolidays
{
    public class CalendarHolidayResponse
    {
        public DateTime CalendarDate { get; set; }
        public string Description { get; set; }
    }
}
