using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Reports
{
    public class ReportEmployeeResponse
    {
        public int TotalEmployee { get; set; }
        public List<GroupReportEmployeeInfo> GroupEmployeeInfo { get; set; }
    }

    public class GroupReportEmployeeInfo
    {
        public string DepartmentName { get; set; }
        public List<ReportEmployeeInfo> EmployeeInfo { get; set; }
    }

    public class ReportEmployeeInfo
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string NSS { get; set; } 
        public string ARS { get; set; } 
        public string AFP { get; set; }
        public string Country { get; set; }
        public DateTime StartWorkDate { get; set; }
        public DateTime EndWorkDate { get; set; }

        public string PositionName { get; set; }
        public string DocumentNumber { get; set; }
    }
}
