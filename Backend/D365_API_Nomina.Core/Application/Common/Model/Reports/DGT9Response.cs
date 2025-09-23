using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Reports
{
    public class DGT9Response
    {
        public string ResgisterType { get; set; } = "E";
        public string Process { get; set; } = "T9";
        public string RNC { get; set; }
        public string Period { get; set; }
        public string StartDate { get; set; }
        public string Duration { get; set; }
        public string CauseSuspension { get; set; }


        public List<DGT9Detail> Details { get; set; }

        public string ResgisterTypeSummary { get; set; } = "S";
        public string RegisterQty { get; set; }
    }

    public class DGT9Detail
    {
        public string EmployeeName { get; set; }
        public string LastName { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string LocationId { get; set; }
        public string Province { get; set; }
        public string Address { get; set; }
    }
}
