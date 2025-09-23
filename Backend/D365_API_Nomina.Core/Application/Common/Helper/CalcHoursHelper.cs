using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Helper
{
    public static class CalcHoursHelper
    {
        public static decimal GetQtyHour(TimeSpan timespan)
        {
            decimal hour = 0;
            hour = (decimal)timespan.TotalMinutes/60;

            return hour;
        }
    }
}
