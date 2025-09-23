using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Reports
{
    public class TXTModelDGT2
    {
        public string ResgisterType { get; set; } = "E";
        public string Process { get; set; } = "T2";
        public string RNC { get; set; }
        public string Period { get; set; }

        public List<TXTModelDGT2Detail> Details { get; set; }

        public string ResgisterTypeSummary { get; set; } = "S";
        public string RegisterQty { get; set; }
    }

    public class TXTModelDGT2Detail
    {
        public string ResgisterType { get; set; } = "D";
        public string ActionType { get; set; } = "NC ";
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string Location { get; set; }
        public string AmountByNormalHour { get; set; }
        public string DayH { get; set; }
        public string Reason { get; set; }
    }
}
