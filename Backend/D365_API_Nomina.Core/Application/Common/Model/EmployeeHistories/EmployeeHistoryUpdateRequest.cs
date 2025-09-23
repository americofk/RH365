using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.EmployeeHistories
{
    public class EmployeeHistoryUpdateRequest
    {
        public string EmployeeHistoryId { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
