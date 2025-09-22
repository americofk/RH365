using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class EmployeeEarningCode
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    public long EarningCodeRefRecID { get; set; }

    public long EmployeeRefRecID { get; set; }

    public long PayrollRefRecID { get; set; }

    public long? PayrollProcessRefRecID { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal IndexEarning { get; set; }

    public int Quantity { get; set; }

    [StringLength(200)]
    public string? Comment { get; set; }

    public int QtyPeriodForPaid { get; set; }

    public int StartPeriodForPaid { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal IndexEarningMonthly { get; set; }

    public int PayFrecuency { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal IndexEarningDiary { get; set; }

    public bool IsUseDGT { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal IndexEarningHour { get; set; }

    public bool IsUseCalcHour { get; set; }

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

    [ForeignKey("EarningCodeRefRecID")]
    [InverseProperty("EmployeeEarningCodes")]
    public virtual EarningCode EarningCodeRefRec { get; set; } = null!;

    [ForeignKey("EmployeeRefRecID")]
    [InverseProperty("EmployeeEarningCodes")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;

    [ForeignKey("PayrollProcessRefRecID")]
    [InverseProperty("EmployeeEarningCodes")]
    public virtual PayrollsProcess? PayrollProcessRefRec { get; set; }

    [ForeignKey("PayrollRefRecID")]
    [InverseProperty("EmployeeEarningCodes")]
    public virtual Payroll PayrollRefRec { get; set; } = null!;
}
