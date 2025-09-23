using D365_API_Nomina.Core.Domain.Common;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class Loan: AuditableCompanyEntity
    {
        public string LoanId { get; set; }
        public string Name { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime ValidFrom { get; set; }
        public decimal MultiplyAmount { get; set; }
        public string LedgerAccount { get; set; }
        public string Description { get; set; }

        public PayFrecuency PayFrecuency { get; set; }
        public IndexBase IndexBase { get; set; }
        public string DepartmentId { get; set; }
        public string ProjCategoryId { get; set; }
        public string ProjId { get; set; }
        public bool LoanStatus { get; set; } = true;

        

    }
}
