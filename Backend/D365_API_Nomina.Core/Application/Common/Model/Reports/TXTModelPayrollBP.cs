using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Reports
{
    public class TXTModelPayrollBP
    {
        public string Type { get; set; } = "H";
        public string RNC { get; set; }
        public string CompanyName { get; set; }
        public string Sequence { get; set; }
        public string ServiceType { get; set; }
        public string EfectiveDate { get; set; }
        public string QtyDebit { get; set; }
        public string TotalAmountDebit { get; set; }
        public string QtyCredit { get; set; }
        public string TotalAmountCredit { get; set; }
        public string NoMID { get; set; }
        public string SendDate { get; set; }
        public string Hour { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Filler { get; set; }

        public List<TXTModelPayrollDetailBP> Details { get; set; }
    }

    public class TXTModelPayrollDetailBP
    {
        public string Type { get; set; } = "N";
        public string RNC { get; set; }
        public string Sequence { get; set; }
        public string TransactionSequence { get; set; }
        public string ToAccount { get; set; }
        public string ToAccountType { get; set; }
        public string Currency { get; set; } = "214";
        public string ToBankCode { get; set; } = "10101070";
        public string ToVerficatorDigitBank { get; set; } = "8";
        public string OperationCode { get; set; } = "32";
        public string TransactionAmount { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public string EmployeeName { get; set; }
        public string ReferenceNumber { get; set; }
        public string Description { get; set; }
        public string DueDate { get; set; }
        public string ContactForm { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeePhone { get; set; }
        public string PaymentProcess { get; set; }
        public string EmptyValue { get; set; }
        public string Filler { get; set; }
    }
}
