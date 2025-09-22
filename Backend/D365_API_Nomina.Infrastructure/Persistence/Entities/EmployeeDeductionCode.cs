using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class EmployeeDeductionCode
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    public long DeductionCodeRefRecID { get; set; }

    public long EmployeeRefRecID { get; set; }

    public long PayrollRefRecID { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal IndexDeduction { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PercentDeduction { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PercentContribution { get; set; }

    [StringLength(200)]
    public string? Comment { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal DeductionAmount { get; set; }

    public int PayFrecuency { get; set; }

    public int QtyPeriodForPaid { get; set; }

    public int StartPeriodForPaid { get; set; }

    [StringLength(500)]
    public string? Observations { get; set; }

    [StringLength(10)]
    public string DataareaID { get; set; } = null!;

    [StringLength(50)]
    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    [StringLength(50)]
    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    [ForeignKey("DeductionCodeRefRecID")]
    [InverseProperty("EmployeeDeductionCodes")]
    public virtual DeductionCode DeductionCodeRefRec { get; set; } = null!;

    [ForeignKey("EmployeeRefRecID")]
    [InverseProperty("EmployeeDeductionCodes")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;

    [ForeignKey("PayrollRefRecID")]
    [InverseProperty("EmployeeDeductionCodes")]
    public virtual Payroll PayrollRefRec { get; set; } = null!;
}
