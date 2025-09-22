using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class EmployeeDepartment : AuditableCompanyEntity
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    public long EmployeeRefRecID { get; set; }

    public long DepartmentRefRecID { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public bool EmployeeDepartmentStatus { get; set; }

    [StringLength(200)]
    public string? Comment { get; set; }

    [ForeignKey("DepartmentRefRecID")]
    [InverseProperty("EmployeeDepartments")]
    public virtual Department DepartmentRefRec { get; set; } = null!;

    [ForeignKey("EmployeeRefRecID")]
    [InverseProperty("EmployeeDepartments")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;
}
