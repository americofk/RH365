using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class Department
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public string DepartmentCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int QtyWorkers { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Description { get; set; }

    public string? Observations { get; set; }

    public bool DepartmentStatus { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<EarningCode> EarningCodes { get; set; } = new List<EarningCode>();

    public virtual ICollection<EmployeeDepartment> EmployeeDepartments { get; set; } = new List<EmployeeDepartment>();

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

    public virtual ICollection<PayrollProcessDetail> PayrollProcessDetails { get; set; } = new List<PayrollProcessDetail>();

    public virtual ICollection<Position> Positions { get; set; } = new List<Position>();

    public virtual ICollection<Taxis> Taxes { get; set; } = new List<Taxis>();
}
