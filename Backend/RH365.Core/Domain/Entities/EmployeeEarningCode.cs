using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class EmployeeEarningCode
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public long EarningCodeRefRecId { get; set; }

    public long EmployeeRefRecId { get; set; }

    public long PayrollRefRecId { get; set; }

    public long? PayrollProcessRefRecId { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public decimal IndexEarning { get; set; }

    public int Quantity { get; set; }

    public string? Comment { get; set; }

    public int QtyPeriodForPaid { get; set; }

    public int StartPeriodForPaid { get; set; }

    public decimal IndexEarningMonthly { get; set; }

    public int PayFrecuency { get; set; }

    public decimal IndexEarningDiary { get; set; }

    public bool IsUseDgt { get; set; }

    public decimal IndexEarningHour { get; set; }

    public bool IsUseCalcHour { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual EarningCode EarningCodeRefRec { get; set; } = null!;

    public virtual Employee EmployeeRefRec { get; set; } = null!;

    public virtual PayrollsProcess? PayrollProcessRefRec { get; set; }

    public virtual Payroll PayrollRefRec { get; set; } = null!;
}
