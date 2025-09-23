using D365_API_Nomina.Core.Domain.Common;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class EmployeeLoan: AuditableCompanyEntity
    {
        public int InternalId { get; set; }

        public string LoanId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime ValidFrom { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public string PayrollId { get; set; }

        public int TotalDues { get; set; }
        public int PendingDues { get; set; }
        public decimal AmountByDues { get; set; }

        public int QtyPeriodForPaid { get; set; }
        public int StartPeriodForPaid { get; set; }
    }
}
