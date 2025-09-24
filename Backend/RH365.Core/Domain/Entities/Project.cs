using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class Project
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public string ProjectCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? LedgerAccount { get; set; }

    public bool ProjectStatus { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

    public virtual ICollection<ProjectCategory> ProjectCategories { get; set; } = new List<ProjectCategory>();

    public virtual ICollection<Taxis> Taxes { get; set; } = new List<Taxis>();
}
