using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class EmployeePosition
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public long EmployeeRefRecId { get; set; }

    public long PositionRefRecId { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public bool EmployeePositionStatus { get; set; }

    public string? Comment { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Employee EmployeeRefRec { get; set; } = null!;

    public virtual Position PositionRefRec { get; set; } = null!;
}
