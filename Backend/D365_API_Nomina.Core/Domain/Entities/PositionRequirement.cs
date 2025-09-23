using D365_API_Nomina.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class PositionRequirement: AuditableCompanyEntity
    {
        public string Name { get; set; }
        public string Detail { get; set; }
        public string PositionId { get; set; }
    }
}
