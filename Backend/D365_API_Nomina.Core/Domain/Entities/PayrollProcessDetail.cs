using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class PayrollProcessDetail : AuditableCompanyEntity
{
   
    public long RecID { get; set; }

    public int ID { get; set; }

    public long PayrollProcessRefRecID { get; set; }

    public long EmployeeRefRecID { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalTaxAmount { get; set; }

    public int PayMethod { get; set; }

    [StringLength(30)]
    public string? BankAccount { get; set; }

    [StringLength(100)]
    public string? BankName { get; set; }

    [StringLength(30)]
    public string? Document { get; set; }

    public long? DepartmentRefRecID { get; set; }

    [StringLength(60)]
    public string? DepartmentName { get; set; }

    public int PayrollProcessStatus { get; set; }

    [StringLength(50)]
    public string? EmployeeName { get; set; }

    public DateTime StartWorkDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalTssAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalTssAndTaxAmount { get; set; }

    [ForeignKey("DepartmentRefRecID")]
    [InverseProperty("PayrollProcessDetails")]
    public virtual Department? DepartmentRefRec { get; set; }

    [ForeignKey("EmployeeRefRecID")]
    [InverseProperty("PayrollProcessDetails")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;

    [ForeignKey("PayrollProcessRefRecID")]
    [InverseProperty("PayrollProcessDetails")]
    public virtual PayrollsProcess PayrollProcessRefRec { get; set; } = null!;
}
