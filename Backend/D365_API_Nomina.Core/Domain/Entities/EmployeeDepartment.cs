using D365_API_Nomina.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class EmployeeDepartment: AuditableCompanyEntity
    {
        public string EmployeeId { get; set; }
        public string DepartmentId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool EmployeeDepartmentStatus { get; set; }
        public string Comment { get; set; }

    }
}
