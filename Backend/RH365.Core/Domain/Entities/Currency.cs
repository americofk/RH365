using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class Currency
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public string CurrencyCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    public virtual ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();

    public virtual ICollection<Taxis> Taxes { get; set; } = new List<Taxis>();
}
