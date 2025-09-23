using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.PayCycles
{
    public class PayCycleResponse
    {
        public int PayCycleId { get; set; }
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }
        public DateTime DefaultPayDate { get; set; }
        public DateTime PayDate { get; set; }
        public decimal AmountPaidPerPeriod { get; set; }
        public StatusPeriod StatusPeriod { get; set; }

        public string PayrollId { get; set; }
        public bool IsForTax { get; set; }

        public PayFrecuency PayFrecuency { get; set; }
        //Modificación para calcular el tss
        public bool IsForTss { get; set; }
    }
}
