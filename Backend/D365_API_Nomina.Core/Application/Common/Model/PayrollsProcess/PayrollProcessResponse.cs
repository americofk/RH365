using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.PayrollsProcess
{
    public class PayrollProcessResponse
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

        public int EmployeeQuantityForPay { get; set; }

        public PayrollProcessStatus PayrollProcessStatus { get; set; }

        public List<PayrollProcessDetail> PayrollProcessDetails { get; set; }

        public bool IsRoyaltyPayroll { get; set; }

        //Campos de tarjetas de totales
        public decimal TotalEarnings { get; set; }
        public decimal TotalExtraHours { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal TotalTaxes { get; set; }
        public decimal TotalLoans { get; set; }
        public decimal Total { get; set; }
    }
}
