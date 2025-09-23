using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Enums
{
    public enum PayrollActionType
    {
        Earning = 0,
        Deduction = 1,
        Tax = 2,
        Loan = 3,
        Cooperative = 4,
        Contribution = 5,
        ExtraHours = 6
    }
}
