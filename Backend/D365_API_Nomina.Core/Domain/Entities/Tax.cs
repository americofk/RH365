using D365_API_Nomina.Core.Domain.Common;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class Tax: AuditableCompanyEntity
    {
        public string TaxId { get; set; }
        public string Name { get; set; }
        public string LedgerAccount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Currency { get; set; }
        public decimal MultiplyAmount { get; set; }
        public PayFrecuency PayFrecuency { get; set; }
        public string Description { get; set; }
        public string LimitPeriod { get; set; }
        public decimal LimitAmount { get; set; }
        public IndexBase IndexBase { get; set; }

        public string ProjId { get; set; }
        public string ProjCategory { get; set; }
        public string DepartmentId { get; set; }
        public bool TaxStatus { get; set; } = true;
    }
}
