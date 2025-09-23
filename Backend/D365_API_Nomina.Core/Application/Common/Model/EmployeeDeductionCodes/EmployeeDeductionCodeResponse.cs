using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.EmployeeDeductionCodes
{
    public class EmployeeDeductionCodeResponse
    {
        public string DeductionCodeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal IndexDeduction { get; set; }
        public decimal PercentDeduction { get; set; }
        public decimal PercentContribution { get; set; }
        public string PayrollId { get; set; }
        public string Comment { get; set; }
        public string EmployeeId { get; set; }

        public string PayrollName { get; set; }
        public string DeductionName { get; set; }

        public decimal DeductionAmount { get; set; }

        //Actualización deducciones por período
        public int QtyPeriodForPaid { get; set; }
        public int StartPeriodForPaid { get; set; }

        public PayFrecuency PayFrecuency { get; set; }
    }
}
