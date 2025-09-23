using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;

namespace D365_API_Nomina.Core.Application.Common.Model.Reports
{
    public class ReportPayrollProcessResponse
    {
        public string PayrollProcessId { get; set; }
        public string PayrollName { get; set; }
        public string Period { get; set; }
        public int TotalEmployee { get; set; }
        public DateTime PaymentDate { get; set; }
        public string ProjId { get; set; }
        public decimal Total { get; set; }

        public List<GroupReportPayrollEmployeeInfo> DepartmentGroups { get; set; }

        //Totales
        public decimal Salary { get; set; }
        public decimal ExtraHour { get; set; }
        public decimal Commision { get; set; }
        public decimal OtherEarning { get; set; }
        public decimal TotalEarning { get; set; }

        public decimal ISR { get; set; }
        public decimal AFP { get; set; }
        public decimal SFS { get; set; }
        public decimal LoanCooperative { get; set; }
        public decimal Loan { get; set; }
        public decimal OtherDiscount { get; set; }
        public decimal TotalDiscount { get; set; }

        //Actualización
        public decimal DeductionCooperative { get; set; }
    }

    public class GroupReportPayrollEmployeeInfo 
    {
        public string DepartmentName { get; set; }
        public List<ReportPayrollEmployeeInfo> Details { get; set; }

        //Totales
        public decimal Salary { get; set; }
        public decimal ExtraHour { get; set; }
        public decimal Commision { get; set; }
        public decimal OtherEarning { get; set; }
        public decimal TotalEarning { get; set; }

        public decimal ISR { get; set; }
        public decimal AFP { get; set; }
        public decimal SFS { get; set; }
        public decimal LoanCooperative { get; set; }
        public decimal Loan { get; set; }
        public decimal OtherDiscount { get; set; }
        public decimal TotalDiscount { get; set; }

        public decimal TotalAmount { get; set; }

        //Actualización
        public decimal DeductionCooperative { get; set; }
    }

    public class ReportPayrollEmployeeInfo
    {
        public string EmployeeId{ get; set; }
        public string EmployeeName { get; set; }

        public decimal Salary { get; set; }
        public decimal ExtraHour { get; set; }
        public decimal Commision { get; set; }
        public decimal OtherEarning { get; set; }
        public decimal TotalEarning { get; set; }

        public decimal ISR { get; set; }
        public decimal AFP { get; set; }
        public decimal SFS { get; set; }
        public decimal LoanCooperative { get; set; }
        public decimal Loan { get; set; }
        public decimal OtherDiscount { get; set; }
        public decimal TotalDiscount { get; set; }

        public decimal TotalAmount { get; set; }

        //Actualización
        public decimal DeductionCooperative { get; set; }
    }
}
