using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class PayCycle
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public int PayCycleId { get; set; }

    public long PayrollRefRecId { get; set; }

    public DateTime PeriodStartDate { get; set; }

    public DateTime PeriodEndDate { get; set; }

    public DateTime DefaultPayDate { get; set; }

    public DateTime PayDate { get; set; }

    public decimal AmountPaidPerPeriod { get; set; }

    public int StatusPeriod { get; set; }

    public bool IsForTax { get; set; }

    public bool IsForTss { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Payroll PayrollRefRec { get; set; } = null!;
}
