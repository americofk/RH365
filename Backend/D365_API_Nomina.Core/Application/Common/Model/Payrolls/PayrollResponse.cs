using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Payrolls
{
    public class PayrollResponse
    {
        public string PayrollId { get; set; }
        public string Name { get; set; }
        public PayFrecuency PayFrecuency { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Description { get; set; }

        public bool IsRoyaltyPayroll { get; set; }

        public string CurrencyId { get; set; }

        public List<PayCycle> PayCycles { get; set; }

        public bool IsForHourPayroll { get; set; }

    }
}
