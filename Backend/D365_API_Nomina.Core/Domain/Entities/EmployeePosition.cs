using D365_API_Nomina.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class EmployeePosition: AuditableCompanyEntity
    {
        public string EmployeeId { get; set; }
        public string PositionId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool EmployeePositionStatus { get; set; } = true;
        public string Comment { get; set; }
    }
}
