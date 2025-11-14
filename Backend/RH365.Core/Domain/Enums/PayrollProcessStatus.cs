using System;
using System.Collections.Generic;
using System.Text;

namespace RH365.Core.Domain.Enums
{
    public enum PayrollProcessStatus
    {
        Created = 0,
        Processed = 1,
        Calculated = 2,
        Paid = 3,
        Closed = 4,
        Canceled = 5

    }
}
