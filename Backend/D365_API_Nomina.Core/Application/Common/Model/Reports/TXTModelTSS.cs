using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Reports
{

    public class TXTModelTSS
    {
        public string RegisterType { get; set; } = "E";
        public string Process { get; set; } = "AM";
        public string RNC { get; set; }
        public string Period { get; set; }

        public List<TXTModelTSSDetail> Details { get; set; }

        public string ResgisterTypeSummary { get; set; } = "S";
        public string RegisterQty { get; set; }
    }

    public class TXTModelTSSDetail
    {
        public string ResgisterType { get; set; } = "D";
        public string PayrollCode { get; set; } = "001";
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeLastName { get; set; }
        public string EmployeeSecondLastName { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public string Salary { get; set; }
        public string Salary_ISR { get; set; }
        public string EmptyAmount { get; set; }
    }
}
