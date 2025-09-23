using System;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class EmployeeHistory
    {
        public string EmployeeHistoryId { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime RegisterDate { get; set; }
        public string EmployeeId { get; set; }

        public bool IsUseDGT { get; set; } = true;
    }
}
