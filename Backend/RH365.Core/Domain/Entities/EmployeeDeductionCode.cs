using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class EmployeeDeductionCode
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public long DeductionCodeRefRecId { get; set; }

    public long EmployeeRefRecId { get; set; }

    public long PayrollRefRecId { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public decimal IndexDeduction { get; set; }

    public decimal PercentDeduction { get; set; }

    public decimal PercentContribution { get; set; }

    public string? Comment { get; set; }

    public decimal DeductionAmount { get; set; }

    public int PayFrecuency { get; set; }

    public int QtyPeriodForPaid { get; set; }

    public int StartPeriodForPaid { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual DeductionCode DeductionCodeRefRec { get; set; } = null!;

    public virtual Employee EmployeeRefRec { get; set; } = null!;

    public virtual Payroll PayrollRefRec { get; set; } = null!;
}
