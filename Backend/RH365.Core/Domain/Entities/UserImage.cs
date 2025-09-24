using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class UserImage
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public long UserRefRecId { get; set; }

    public byte[]? Image { get; set; }

    public string Extension { get; set; } = null!;

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual User UserRefRec { get; set; } = null!;
}
