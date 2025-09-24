using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class Taxis
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public string TaxCode { get; set; } = null!;

    public string? Name { get; set; }

    public string? LedgerAccount { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    public long CurrencyRefRecId { get; set; }

    public decimal MultiplyAmount { get; set; }

    public int PayFrecuency { get; set; }

    public string? Description { get; set; }

    public string? LimitPeriod { get; set; }

    public decimal LimitAmount { get; set; }

    public int IndexBase { get; set; }

    public long? ProjectRefRecId { get; set; }

    public long? ProjectCategoryRefRecId { get; set; }

    public long? DepartmentRefRecId { get; set; }

    public bool TaxStatus { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Currency CurrencyRefRec { get; set; } = null!;

    public virtual Department? DepartmentRefRec { get; set; }

    public virtual ICollection<EmployeeTaxis> EmployeeTaxes { get; set; } = new List<EmployeeTaxis>();

    public virtual ProjectCategory? ProjectCategoryRefRec { get; set; }

    public virtual Project? ProjectRefRec { get; set; }

    public virtual ICollection<TaxDetail> TaxDetails { get; set; } = new List<TaxDetail>();
}
