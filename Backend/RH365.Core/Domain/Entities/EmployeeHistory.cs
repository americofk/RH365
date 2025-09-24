using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class EmployeeHistory
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public string EmployeeHistoryCode { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime RegisterDate { get; set; }

    public long EmployeeRefRecId { get; set; }

    public bool IsUseDgt { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Employee EmployeeRefRec { get; set; } = null!;
}
