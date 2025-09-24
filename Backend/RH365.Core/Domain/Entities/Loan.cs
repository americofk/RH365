using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class Loan
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public string LoanCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateTime ValidTo { get; set; }

    public DateTime ValidFrom { get; set; }

    public decimal MultiplyAmount { get; set; }

    public string? LedgerAccount { get; set; }

    public string? Description { get; set; }

    public int PayFrecuency { get; set; }

    public int IndexBase { get; set; }

    public long? DepartmentRefRecId { get; set; }

    public long? ProjCategoryRefRecId { get; set; }

    public long? ProjectRefRecId { get; set; }

    public bool LoanStatus { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Department? DepartmentRefRec { get; set; }

    public virtual ICollection<EmployeeLoanHistory> EmployeeLoanHistories { get; set; } = new List<EmployeeLoanHistory>();

    public virtual ICollection<EmployeeLoan> EmployeeLoans { get; set; } = new List<EmployeeLoan>();

    public virtual ProjectCategory? ProjCategoryRefRec { get; set; }

    public virtual Project? ProjectRefRec { get; set; }
}
