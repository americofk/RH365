using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Helpers
{
    public class EmpPayrollProcessHelper
    {
        public string EmployeeId { get; set; }
        public PayMethod PayMethod { get; set; }
        public string EmployeeName { get; set; }
        public DateTime StartWorkDate { get; set; }
    }
}
