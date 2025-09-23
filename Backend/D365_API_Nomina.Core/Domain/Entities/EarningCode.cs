using D365_API_Nomina.Core.Domain.Common;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class EarningCode : AuditableCompanyEntity
    {
       

        public string EarningCodeId { get; set; }
        public string Name { get; set; }
        public bool IsTSS { get; set; }
        public bool IsISR { get; set; }
        public bool IsExtraHours { get; set; }
        public bool IsUseDGT { get; set; }

        public string ProjId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Description { get; set; }
        public IndexBase IndexBase { get; set; }
        public decimal MultiplyAmount { get; set; }
        public string LedgerAccount { get; set; }
        public string Department { get; set; }

        public bool EarningCodeStatus { get; set; } = true;

        //Actualización
        public bool IsRoyaltyPayroll { get; set; }

        //Actualización para cálculo automático de horas extras
        public bool IsHoliday { get; set; }
        public TimeSpan WorkFrom { get; set; }
        public TimeSpan WorkTo { get; set; }
    }
}
