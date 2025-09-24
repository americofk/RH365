using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class PayrollProcessDetail
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public long PayrollProcessRefRecId { get; set; }

    public long EmployeeRefRecId { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal TotalTaxAmount { get; set; }

    public int PayMethod { get; set; }

    public string? BankAccount { get; set; }

    public string? BankName { get; set; }

    public string? Document { get; set; }

    public long? DepartmentRefRecId { get; set; }

    public string? DepartmentName { get; set; }

    public int PayrollProcessStatus { get; set; }

    public string? EmployeeName { get; set; }

    public DateTime StartWorkDate { get; set; }

    public decimal TotalTssAmount { get; set; }

    public decimal TotalTssAndTaxAmount { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Department? DepartmentRefRec { get; set; }

    public virtual Employee EmployeeRefRec { get; set; } = null!;

    public virtual PayrollsProcess PayrollProcessRefRec { get; set; } = null!;
}
