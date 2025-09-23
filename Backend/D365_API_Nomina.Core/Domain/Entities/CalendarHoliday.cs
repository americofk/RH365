using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class CalendarHoliday
    {
        public DateTime CalendarDate { get; set; }
        public string Description { get; set; }
    }
}
