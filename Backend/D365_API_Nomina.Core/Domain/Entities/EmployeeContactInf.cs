using D365_API_Nomina.Core.Domain.Common;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class EmployeeContactInf: AuditableCompanyEntity
    {
        public int InternalId { get; set; }
        public string NumberAddress { get; set; }
        public string Comment { get; set; }
        public bool IsPrincipal { get; set; }
        public string EmployeeId { get; set; }
        public ContactType ContactType { get; set; }
    }
}
