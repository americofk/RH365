using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Reports
{
    public class DGT5Response
    {
        public string ResgisterType { get; set; } = "E";
        public string Process { get; set; } = "T5";
        public string RNC { get; set; }
        public string Period { get; set; }

        public List<DGT5Detail> Details { get; set; }

        public string ResgisterTypeSummary { get; set; } = "S";
        public string RegisterQty { get; set; }
    }

    public class DGT5Detail
    {
        public string NoveltyType { get; set; }
        public string EmployeeName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public string Gender { get; set; }
        public string Salary { get; set; }
        public string AdmissionDate { get; set; }
        public string DocumentNumber { get; set; }

        public string SalaryDay { get; set; }
        public string WorkedDay { get; set; }
    }
}
