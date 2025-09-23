using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Reports
{
    public class ReportConfigRequest
    {
        public string Salary { get; set; }
        public string Comission { get; set; }
        public string AFP { get; set; }
        public string SFS { get; set; }
        public string LoanCooperative { get; set; }

        //Actualización abono de cooperativa
        public string DeductionCooperative { get; set; }
    }
}
