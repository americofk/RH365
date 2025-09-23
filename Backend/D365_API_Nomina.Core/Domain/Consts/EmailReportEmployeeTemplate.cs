using D365_API_Nomina.Core.Application.Common.Model.Reports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Consts
{
    public static class EmailReportEmployeeTemplate
    {
        public static string Template(ReportPayrollPaymentResponse model, string partialRoute)
        {
            string path = Path.Combine(partialRoute, @$"Resources\EmployeeTemplate.txt");
            string templatetext = string.Empty;

            using (StreamReader fs = new StreamReader(path))
            {
                templatetext = fs.ReadToEnd();
            }

            templatetext = templatetext.Replace("V-ReportDate", DateTime.Now.ToString("dd/MM/yyyy"));
            templatetext = templatetext.Replace("V-EmployeeName", model.EmployeeName);
            templatetext = templatetext.Replace("V-StartWorkDate", model.StartWorkDate.ToString("dd/MM/yyyy"));
            templatetext = templatetext.Replace("V-DocumentNum", model.Document);
            templatetext = templatetext.Replace("V-Department", model.Department);
            templatetext = templatetext.Replace("V-PositionName", model.PositionName);
            templatetext = templatetext.Replace("V-PayrollName", model.PayrollName);
            templatetext = templatetext.Replace("V-Period", model.Period);
            templatetext = templatetext.Replace("V-PaymentDate", model.PaymentDate.ToString("dd/MM/yyyy"));
            templatetext = templatetext.Replace("V-PayMethod", model.PayMethod.ToString());
            templatetext = templatetext.Replace("V-Salary", model.Salary.ToString());
            templatetext = templatetext.Replace("V-Loan", model.Loan.ToString());
            templatetext = templatetext.Replace("V-AFP", model.AFP.ToString());
            templatetext = templatetext.Replace("V-SFS", model.SFS.ToString());
            templatetext = templatetext.Replace("V-ISR", model.ISR.ToString());
            templatetext = templatetext.Replace("V-Comission", model.Commision.ToString());
            templatetext = templatetext.Replace("V-DeductionCooperative", model.DeductionCooperative.ToString());
            templatetext = templatetext.Replace("V-LCooperative", model.LoanCooperative.ToString());
            templatetext = templatetext.Replace("V-ExtraHour", model.ExtraHour.ToString());
            templatetext = templatetext.Replace("V-OtherEarning", model.OtherEarning.ToString());
            templatetext = templatetext.Replace("V-OtherDiscount", model.OtherDiscount.ToString());
            templatetext = templatetext.Replace("V-Total", model.Total.ToString());

            return templatetext;
        }
    }
}
