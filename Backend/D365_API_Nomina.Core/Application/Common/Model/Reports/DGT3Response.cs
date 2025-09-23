using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Reports
{
    public class DGT3Response
    {
        public string ResgisterType { get; set; } = "E";
        public string Process { get; set; } = "T3";
        public string RNC { get; set; }
        public string Period { get; set; }

        public List<DGT3Detail> Details { get; set; }

        public string ResgisterTypeSummary { get; set; } = "S";
        public string RegisterQty { get; set; }
    }

    public class DGT3Detail
    {
        public string EmployeeName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public string Gender { get; set; }
        public string Salary { get; set; }
        public string AdmissionDate { get; set; }
        public string DocumentNumber { get; set; }
    }
}
