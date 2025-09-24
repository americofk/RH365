using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class ProjectCategory
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public string? LedgerAccount { get; set; }

    public long ProjectRefRecId { get; set; }

    public bool ProjectCategoryStatus { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

    public virtual Project ProjectRefRec { get; set; } = null!;

    public virtual ICollection<Taxis> Taxes { get; set; } = new List<Taxis>();
}
