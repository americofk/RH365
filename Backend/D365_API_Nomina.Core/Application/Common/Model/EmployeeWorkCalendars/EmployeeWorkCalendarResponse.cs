using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.EmployeeWorkCalendars
{
    public class EmployeeWorkCalendarResponse
    {
        public string EmployeeId { get; set; }
        public DateTime CalendarDate { get; set; }
        public string CalendarDay { get; set; }
        public int InternalId { get; set; }
        public TimeSpan WorkFrom { get; set; }
        public TimeSpan WorkTo { get; set; }
        public TimeSpan BreakWorkFrom { get; set; }
        public TimeSpan BreakWorkTo { get; set; }
    }
}
