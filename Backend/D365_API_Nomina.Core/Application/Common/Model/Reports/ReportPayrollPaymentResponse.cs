using D365_API_Nomina.Core.Domain.Enums;
using System;

namespace D365_API_Nomina.Core.Application.Common.Model.Reports
{
    public class ReportPayrollPaymentResponse
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public DateTime StartWorkDate { get; set; }
        public string Document { get; set; }
        public string Department { get; set; }
        public string PayrollName { get; set; }
        public string Period { get; set; }
        public string PositionName { get; set; }
        public DateTime PaymentDate { get; set; }
        public PayMethod PayMethod { get; set; }

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


        public decimal Total { get; set; }
        public string BankAccount { get; set; }

        //Actualización
        public decimal DeductionCooperative { get; set; }

    }
}
