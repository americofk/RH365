using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class EmployeeLoanHistory
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public long EmployeeLoanRefRecId { get; set; }

    public long LoanRefRecId { get; set; }

    public long EmployeeRefRecId { get; set; }

    public DateTime PeriodStartDate { get; set; }

    public DateTime PeriodEndDate { get; set; }

    public long PayrollRefRecId { get; set; }

    public long? PayrollProcessRefRecId { get; set; }

    public decimal LoanAmount { get; set; }

    public decimal PaidAmount { get; set; }

    public decimal PendingAmount { get; set; }

    public int TotalDues { get; set; }

    public int PendingDues { get; set; }

    public decimal AmountByDues { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual EmployeeLoan EmployeeLoanRefRec { get; set; } = null!;

    public virtual Employee EmployeeRefRec { get; set; } = null!;

    public virtual Loan LoanRefRec { get; set; } = null!;

    public virtual PayrollsProcess? PayrollProcessRefRec { get; set; }

    public virtual Payroll PayrollRefRec { get; set; } = null!;
}
