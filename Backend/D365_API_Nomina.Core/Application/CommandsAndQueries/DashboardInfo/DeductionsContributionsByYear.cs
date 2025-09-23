using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.DashboardInfo
{
    public class DeductionsContributionsByYear
    {
        public List<string> Keys { get; set; }
        public List<decimal> CtbutionValues { get; set; }
        public List<decimal> DtbutionValues { get; set; }
    }
}
