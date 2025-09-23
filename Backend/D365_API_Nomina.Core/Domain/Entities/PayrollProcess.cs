using D365_API_Nomina.Core.Domain.Common;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class PayrollProcess: AuditableCompanyEntity
    {
        public string PayrollProcessId { get; set; }
        public string PayrollId { get; set; }
        public string Description { get; set; }
        public DateTime PaymentDate { get; set; }
        public int EmployeeQuantity { get; set; }

        public string ProjId { get; set; }
        public string ProjCategoryId { get; set; }
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }

        public int PayCycleId { get; set; }

        //Campos agregados fuera del diseño
        public int EmployeeQuantityForPay { get; set; }
        //public string PrevProcessId { get; set; }
        public PayrollProcessStatus PayrollProcessStatus { get; set; } = PayrollProcessStatus.Created;



        //Campo de la actualización
        public bool UsedForTax { get; set; }
        public bool IsPayCycleTax { get; set; }

        public decimal TotalAmountToPay { get; set; }

        public bool IsRoyaltyPayroll { get; set; }

        //Modificación para el cálculo de deducciones
        public bool UsedForTss { get; set; }
        public bool IsPayCycleTss { get; set; }


        public bool IsForHourPayroll { get; set; }
    }
}
