using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.EmployeeEarningCodes
{
    public class EmployeeEarningCodeResponse
    {
        public int InternalId { get; set; }
        public string EarningCodeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal IndexEarning { get; set; }
        public int Quantity { get; set; }
        public string PayrollId { get; set; }
        public string Comment { get; set; }
        public string EmployeeId { get; set; }
        public int QtyPeriodForPaid { get; set; }
        public int StartPeriodForPaid { get; set; }

        public string PayrollName { get; set; }
        public string EarningName { get; set; }


        public PayFrecuency PayFrecuency { get; set; }
        public decimal IndexEarningMonthly { get; set; }
        public decimal IndexEarningDiary { get; set; }

        public bool IsUseDGT { get; set; }

        public bool IsUseCalcHour { get; set; }
        public decimal IndexEarningHour { get; set; }
    }
}
