using D365_API_Nomina.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class EmployeeImage: AuditableCompanyEntity
    {
        public byte[] Image { get; set; }
        public string EmployeeId { get; set; }
        public string Extension { get; set; }
    }
}
