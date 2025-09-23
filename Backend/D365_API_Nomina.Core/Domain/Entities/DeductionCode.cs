using D365_API_Nomina.Core.Domain.Common;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class DeductionCode : AuditableCompanyEntity
    {
        public string DeductionCodeId { get; set; }
        public string Name { get; set; }
        public string ProjId { get; set; }
        public string ProjCategory { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Description { get; set; }
        public string LedgerAccount { get; set; }
        public string Department { get; set; }
        public PayrollAction PayrollAction { get; set; }
        public bool DeductionStatus { get; set; } = true;

        public IndexBase Ctbution_IndexBase { get; set; }
        public decimal Ctbution_MultiplyAmount { get; set; }
        public PayFrecuency Ctbution_PayFrecuency { get; set; }
        public PayFrecuency Ctbution_LimitPeriod { get; set; }
        public decimal Ctbution_LimitAmount { get; set; }
        public decimal Ctbution_LimitAmountToApply { get; set; }

        public IndexBase Dduction_IndexBase { get; set; }
        public decimal Dduction_MultiplyAmount { get; set; }
        public PayFrecuency Dduction_PayFrecuency { get; set; }
        public PayFrecuency Dduction_LimitPeriod { get; set; }
        public decimal Dduction_LimitAmount { get; set; }
        public decimal Dduction_LimitAmountToApply { get; set; }

        //Actualización
        public bool IsForTaxCalc { get; set; }

        //Modificación para el calculo de las deducciones
        public bool IsForTssCalc { get; set; }


    }
}
