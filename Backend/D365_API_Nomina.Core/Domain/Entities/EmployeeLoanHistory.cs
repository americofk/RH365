using D365_API_Nomina.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class EmployeeLoanHistory : AuditableCompanyEntity
    {
        public int InternalId { get; set; }
        public int ParentInternalId { get; set; }

        public string LoanId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }

        public string PayrollId { get; set; }
        public string PayrollProcessId { get; set; }

        public decimal LoanAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PendingAmount { get; set; }

        public int TotalDues { get; set; }
        public int PendingDues { get; set; }
        public decimal AmountByDues { get; set; }
    }
}
