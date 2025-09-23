using D365_API_Nomina.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class ReportConfig: AuditableCompanyEntity
    {
        public int InternalId { get; set; }
        public string Salary { get; set; }
        public string Comission { get; set; }
        public string AFP { get; set; }
        public string SFS { get; set; }
        public string LoanCooperative { get; set; }

        //Actualización abono de cooperativa
        public string DeductionCooperative { get; set; }
    }
}
