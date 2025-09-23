using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Reports
{
    public class DGT2Response
    {
        public string ResgisterType { get; set; } = "E";
        public string Process { get; set; } = "T2";
        public string RNC { get; set; }
        public string Period { get; set; }

        public List<DGT2Detail> Details { get; set; }

        public string ResgisterTypeSummary { get; set; } = "S";
        public string RegisterQty { get; set; }
    }

    public class DGT2Detail
    {
        public string EmployeeName { get; set; }
        public decimal QtyExtraHour { get; set; }
        public decimal TotalAmountExtraHour { get; set; }
    }
}
