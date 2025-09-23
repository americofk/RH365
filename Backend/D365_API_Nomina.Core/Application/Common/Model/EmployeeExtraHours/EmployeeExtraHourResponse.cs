using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.EmployeeExtraHours
{
    public class EmployeeExtraHourResponse
    {
        public DateTime WorkedDay { get; set; }
        public TimeSpan StartHour { get; set; }
        public TimeSpan EndHour { get; set; }
        public decimal Amount { get; set; }
        public decimal Indice { get; set; }
        public decimal Quantity { get; set; }
        public StatusExtraHour StatusExtraHour { get; set; }
        public string PayrollId { get; set; }
        public string PayrollName { get; set; }
        public string EarningCodeId { get; set; }
        public string EmployeeId { get; set; }
        public string EarningCodeName { get; set; }
        public DateTime CalcPayrollDate { get; set; }
        public string Comment { get; set; }
    }
}
