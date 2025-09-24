using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class TaxDetail
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public long TaxRefRecId { get; set; }

    public decimal AnnualAmountHigher { get; set; }

    public decimal AnnualAmountNotExceed { get; set; }

    public decimal Percent { get; set; }

    public decimal FixedAmount { get; set; }

    public decimal ApplicableScale { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Taxis TaxRefRec { get; set; } = null!;
}
