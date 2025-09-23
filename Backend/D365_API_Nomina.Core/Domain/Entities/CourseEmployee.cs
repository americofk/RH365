using D365_API_Nomina.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class CourseEmployee: AuditableCompanyEntity
    {
        public string EmployeeId { get; set; }
        public string CourseId { get; set; }
        public string Comment { get; set; }
    }
}
