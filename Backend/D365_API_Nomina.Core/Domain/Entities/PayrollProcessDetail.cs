using D365_API_Nomina.Core.Domain.Common;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class PayrollProcessDetail: AuditableCompanyEntity
    {
        //public string ProcessDetailId { get; set; }
        public string PayrollProcessId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public PayMethod PayMethod { get; set; } //Usarlo para filtrar los que van al documento de pago electrónico
        public string BankAccount { get; set; }
        public string BankName { get; set; }
        public string Document { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime StartWorkDate { get; set; }
        public PayrollProcessStatus PayrollProcessStatus { get; set; }

        //Modificación para el calculo de las deducciones de tss
        public decimal TotalTssAmount { get; set; }
        public decimal TotalTssAndTaxAmount { get; set; }
    }
}
