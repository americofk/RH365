using D365_API_Nomina.Core.Domain.Common;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class PayrollProcessAction: AuditableCompanyEntity
    {
        public int InternalId { get; set; }
        public string PayrollProcessId { get; set; }
        public string EmployeeId { get; set; }

        public PayrollActionType PayrollActionType { get; set; }
        public string ActionName { get; set; }
        public decimal ActionAmount { get; set; }
        public bool ApplyTax { get; set; }
        public bool ApplyTSS { get; set; }
        public bool ApplyRoyaltyPayroll { get; set; }

        public string ActionId { get; set; }
    }
}
