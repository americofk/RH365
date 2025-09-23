using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.EmployeeTaxes
{
    public class EmployeeTaxResponse
    {
        public string TaxId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime ValidFrom { get; set; }
        public string PayrollId { get; set; }
        public string PayrollName { get; set; }
    }
}
