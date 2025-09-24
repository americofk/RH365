using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class EmployeeImage
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public long EmployeeRefRecId { get; set; }

    public byte[]? Image { get; set; }

    public string Extension { get; set; } = null!;

    public bool IsPrincipal { get; set; }

    public string? Comment { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Employee EmployeeRefRec { get; set; } = null!;
}
