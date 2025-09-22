using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class Department
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

    [StringLength(500)]
    public string? Observations { get; set; }

    public bool DepartmentStatus { get; set; }

    [StringLength(10)]
    public string DataareaID { get; set; } = null!;

    [StringLength(50)]
    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    [StringLength(50)]
    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

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
