using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.DashboardInfo
{
    public class DashboardGraphicsInfo
    {
        public EmployeeByDepartments EmployeeByDepartments { get; set; }
        public DeductionsContributionsByYear DtbutionCtbutionByYear { get; set; }
        public AmountByAction AmountByAction { get; set; }
        public TrimestralPayrollAmount TrimestralPayrollAmount { get; set; }
    }
}
