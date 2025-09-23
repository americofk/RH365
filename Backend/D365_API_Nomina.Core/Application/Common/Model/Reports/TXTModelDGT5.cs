using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Reports
{
    public class TXTModelDGT5
    {
        public string ResgisterType { get; set; } = "E";
        public string Process { get; set; } = "T5";
        public string RNC { get; set; }
        public string Period { get; set; }

        public List<TXTModelDGT5Detail> Details { get; set; }

        public string ResgisterTypeSummary { get; set; } = "S";
        public string RegisterQty { get; set; }
    }

    public class TXTModelDGT5Detail
    {
        public string ResgisterType { get; set; } = "D";
        public string ActionType { get; set; } = "NI ";
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }

        public string Salary { get; set; }
        public string SalaryDiary { get; set; }
        public string AdmissionDate { get; set; }
        public string Occupation { get; set; }
        public string OccupationDescription { get; set; }
        public string Turn { get; set; }
        public string Location { get; set; }
        public string EductionalLevel { get; set; }
        public string Disability { get; set; }
        public string WorkedDays { get; set; }
    }
}
