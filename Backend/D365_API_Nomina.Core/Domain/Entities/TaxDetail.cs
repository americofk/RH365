using D365_API_Nomina.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class TaxDetail: AuditableCompanyEntity
    {
        public int InternalId { get; set; }
        //Salario anual superior a 
        public decimal AnnualAmountHigher { get; set; }

        //Salario anual no excede 
        public decimal AnnualAmountNotExceed { get; set; }

        public decimal Percent { get; set; }

        public decimal FixedAmount { get; set; }

        public decimal ApplicableScale { get; set; }

        public string TaxId { get; set; }
    }
}
