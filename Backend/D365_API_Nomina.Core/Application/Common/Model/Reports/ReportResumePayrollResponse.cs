using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;

namespace D365_API_Nomina.Core.Application.Common.Model.Reports
{
    public class ReportResumePayrollResponse
    {
        public string PayrollName { get; set; }
        public string Period { get; set; }
        public int TotalEmployee { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Project { get; set; }
        public decimal Total { get; set; }

        public decimal Salary { get; set; }
        public decimal ExtraHour { get; set; }
        public decimal Commision { get; set; }
        public decimal OtherEarning { get; set; }

        public decimal ISR { get; set; }
        public decimal AFP { get; set; }
        public decimal SFS { get; set; }
        public decimal LoanCooperative { get; set; }
        public decimal Loan { get; set; }
        public decimal OtherDiscount { get; set; }

        public List<PayMethodTotal> PayMethodTotal { get; set; }

        public string DepartmentName { get; set; }

        //Actualización
        public decimal DeductionCooperative { get; set; }
    }

    public class PayMethodTotal
    {
        public PayMethod PayMethod { get; set; }
        public int Total { get; set; }
    }
}
