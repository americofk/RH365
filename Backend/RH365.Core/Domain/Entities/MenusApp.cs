using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class MenusApp
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public string MenuCode { get; set; } = null!;

    public string MenuName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Action { get; set; }

    public string Icon { get; set; } = null!;

    public long? MenuFatherRefRecId { get; set; }

    public int Sort { get; set; }

    public bool IsViewMenu { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<MenusApp> InverseMenuFatherRefRec { get; set; } = new List<MenusApp>();

    public virtual ICollection<MenuAssignedToUser> MenuAssignedToUsers { get; set; } = new List<MenuAssignedToUser>();

    public virtual MenusApp? MenuFatherRefRec { get; set; }
}
