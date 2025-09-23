using D365_API_Nomina.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class EmployeeWorkCalendar: AuditableCompanyEntity
    {
        public int InternalId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime CalendarDate { get; set; }
        public string CalendarDay { get; set; }
        public TimeSpan WorkFrom { get; set; }
        public TimeSpan WorkTo { get; set; }
        public TimeSpan BreakWorkFrom { get; set; }
        public TimeSpan BreakWorkTo { get; set; }
    }
}
