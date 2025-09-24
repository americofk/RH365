using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class EmployeeExtraHour
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public long EmployeeRefRecId { get; set; }

    public long EarningCodeRefRecId { get; set; }

    public long PayrollRefRecId { get; set; }

    public DateTime WorkedDay { get; set; }

    public TimeOnly StartHour { get; set; }

    public TimeOnly EndHour { get; set; }

    public decimal Amount { get; set; }

    public decimal Indice { get; set; }

    public decimal Quantity { get; set; }

    public int StatusExtraHour { get; set; }

    public DateTime CalcPayrollDate { get; set; }

    public string? Comment { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual EarningCode EarningCodeRefRec { get; set; } = null!;

    public virtual Employee EmployeeRefRec { get; set; } = null!;

    public virtual Payroll PayrollRefRec { get; set; } = null!;
}
