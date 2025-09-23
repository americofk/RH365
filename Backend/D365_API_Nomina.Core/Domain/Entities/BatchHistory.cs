using D365_API_Nomina.Core.Domain.Common;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class BatchHistory: AuditableCompanyEntity
    {
        public int InternalId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public BatchEntity BatchEntity { get; set; }
        public string Information { get; set; }
        public bool IsError { get; set; } = false;
        public bool IsFinished { get; set; } = false;

    }
}
