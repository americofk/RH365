using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Reports
{
    public class TXTModelDGT3
    {
        public string ResgisterType { get; set; } = "E";
        public string Process { get; set; } = "T3";
        public string RNC { get; set; }
        public string Period { get; set; }

        public List<TXTModelDGT3Detail> Details { get; set; }

        public string ResgisterTypeSummary { get; set; } = "S";
        public string RegisterQty { get; set; }
    }

    public class TXTModelDGT3Detail
    {
        public string ResgisterType { get; set; } = "D";
        public string ActionType { get; set; } = "NI ";
        public string Name { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string BirthDate { get; set; }
        public string Gender { get; set; }
        public string Salary { get; set; }
        public string AdmissionDate { get; set; }
        public string Occupation { get; set; }
        public string DocumentType { get; set; }
        public string OccupationDescription { get; set; }
        public string StartVacation { get; set; }
        public string EndVacation { get; set; }
        public string Turn { get; set; }
        public string Location { get; set; }
        public string EductionalLevel { get; set; }
        public string Disability { get; set; }
        public string DocumentNumber { get; set; }
    }
}
