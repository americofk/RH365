using D365_API_Nomina.Core.Domain.Common;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class EmployeeBankAccount: AuditableCompanyEntity
    {
        public string EmployeeId { get; set; }
        public int InternalId { get; set; }
        public string BankName { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountNum { get; set; }
        public string Comment { get; set; }
        public bool IsPrincipal { get; set; }
        public string Currency { get; set; }
    }
}
