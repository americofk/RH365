using D365_API_Nomina.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class CompaniesAssignedToUser : AuditableEntity
    {
        public string CompanyId { get; set; }
        public string Alias { get; set; }
    }
}
