using D365_API_Nomina.Core.Domain.Common;
using D365_API_Nomina.Core.Domain.Enums;
using System;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class EmployeeExtraHour: AuditableCompanyEntity
    {
        public DateTime WorkedDay { get; set; }
        //public int StartHour { get; set; }
        //public int EndHour { get; set; }
        public TimeSpan StartHour { get; set; }
        public TimeSpan EndHour { get; set; }
        //public int TotalHour { get; set; }
        //public int TotalExtraHour { get; set; }
        public decimal Amount { get; set; }
        public decimal Indice { get; set; }
        //public int Quantity { get; set; }
        public decimal Quantity { get; set; }
        public StatusExtraHour StatusExtraHour { get; set; } = StatusExtraHour.Open;
        public string PayrollId { get; set; }
        public string EarningCodeId { get; set; }
        public string EmployeeId { get; set; }

        public string Comment { get; set; }

        //Actualización, campo para indicar la fecha de uso de horas extra
        public DateTime CalcPayrollDate { get; set; }

    }
}
