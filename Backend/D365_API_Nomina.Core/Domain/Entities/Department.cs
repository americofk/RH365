using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class Department : AuditableCompanyEntity
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    [StringLength(20)]
    public string DepartmentCode { get; set; } = null!;

    [StringLength(60)]
    public string Name { get; set; } = null!;

    public int QtyWorkers { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [StringLength(100)]
    public string? Description { get; set; }

    [InverseProperty("DepartmentRefRec")]
    public virtual ICollection<EarningCode> EarningCodes { get; set; } = new List<EarningCode>();

    [InverseProperty("DepartmentRefRec")]
    public virtual ICollection<EmployeeDepartment> EmployeeDepartments { get; set; } = new List<EmployeeDepartment>();

    [InverseProperty("DepartmentRefRec")]
    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

    [InverseProperty("DepartmentRefRec")]
    public virtual ICollection<PayrollProcessDetail> PayrollProcessDetails { get; set; } = new List<PayrollProcessDetail>();

    [InverseProperty("DepartmentRefRec")]
    public virtual ICollection<Position> Positions { get; set; } = new List<Position>();

    [InverseProperty("DepartmentRefRec")]
    public virtual ICollection<Taxis> Taxes { get; set; } = new List<Taxis>();
}
