using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Reports
{
    public class ReportTSSFileResponse
    {
        public string ResgisterType { get; set; } = "E";
        public string Process { get; set; } = "AM";
        public string RNC { get; set; }
        public string Period { get; set; }
        public string PayrollName { get; set; }

        public List<TSSFile> Details { get; set; }
    }

    public class TSSFile
    {
        //public string PayrollCode { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeLastName { get; set; }
        //public string EmployeeSecondLastName { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public decimal Salary { get; set; }
        public decimal Salary_ISR { get; set; }
    }
}
