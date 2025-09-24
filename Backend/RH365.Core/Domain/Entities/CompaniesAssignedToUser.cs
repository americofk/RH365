using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class CompaniesAssignedToUser
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public long CompanyRefRecId { get; set; }

    public long UserRefRecId { get; set; }

    public bool IsActive { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Company CompanyRefRec { get; set; } = null!;

    public virtual User UserRefRec { get; set; } = null!;
}
