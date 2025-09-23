using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.EmployeeDeparments
{
    public class EmployeeDepartmentResponse
    {
        public string EmployeeId { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool EmployeeDepartmentStatus { get; set; }
        public string Comment { get; set; }
    }
}
