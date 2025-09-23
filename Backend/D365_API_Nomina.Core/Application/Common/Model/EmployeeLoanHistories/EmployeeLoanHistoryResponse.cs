using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.EmployeeLoans
{
    public class EmployeeLoanHistoryResponse
    {
        public int InternalId { get; set; }

        public string LoanId { get; set; }
        public string LoanName { get; set; }
        public string EmployeeId { get; set; }
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public string PayrollId { get; set; }
        public string PayrollName { get; set; }
        public string PayrollProcessId { get; set; }

        public int TotalDues { get; set; }
        public decimal AmountByDues { get; set; }

    }
}
