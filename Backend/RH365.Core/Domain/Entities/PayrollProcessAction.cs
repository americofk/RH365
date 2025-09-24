using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class PayrollProcessAction
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public long PayrollProcessRefRecId { get; set; }

    public long EmployeeRefRecId { get; set; }

    public int PayrollActionType { get; set; }

    public string ActionName { get; set; } = null!;

    public decimal ActionAmount { get; set; }

    public bool ApplyTax { get; set; }

    public bool ApplyTss { get; set; }

    public bool ApplyRoyaltyPayroll { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Employee EmployeeRefRec { get; set; } = null!;

    public virtual PayrollsProcess PayrollProcessRefRec { get; set; } = null!;
}
