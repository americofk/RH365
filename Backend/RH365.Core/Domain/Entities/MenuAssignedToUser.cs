using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class MenuAssignedToUser
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public long UserRefRecId { get; set; }

    public long MenuRefRecId { get; set; }

    public bool PrivilegeView { get; set; }

    public bool PrivilegeEdit { get; set; }

    public bool PrivilegeDelete { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual MenusApp MenuRefRec { get; set; } = null!;

    public virtual User UserRefRec { get; set; } = null!;
}
