using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class EmployeeLoan
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public long LoanRefRecId { get; set; }

    public long EmployeeRefRecId { get; set; }

    public DateTime ValidTo { get; set; }

    public DateTime ValidFrom { get; set; }

    public decimal LoanAmount { get; set; }

    public int StartPeriodForPaid { get; set; }

    public decimal PaidAmount { get; set; }

    public decimal PendingAmount { get; set; }

    public long PayrollRefRecId { get; set; }

    public int TotalDues { get; set; }

    public int PendingDues { get; set; }

    public int QtyPeriodForPaid { get; set; }

    public decimal AmountByDues { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<EmployeeLoanHistory> EmployeeLoanHistories { get; set; } = new List<EmployeeLoanHistory>();

    public virtual Employee EmployeeRefRec { get; set; } = null!;

    public virtual Loan LoanRefRec { get; set; } = null!;

    public virtual Payroll PayrollRefRec { get; set; } = null!;
}
