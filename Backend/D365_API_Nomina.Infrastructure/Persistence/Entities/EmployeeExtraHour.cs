using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class EmployeeExtraHour
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    public long EmployeeRefRecID { get; set; }

    public long EarningCodeRefRecID { get; set; }

    public long PayrollRefRecID { get; set; }

    public DateTime WorkedDay { get; set; }

    public TimeOnly StartHour { get; set; }

    public TimeOnly EndHour { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Indice { get; set; }

    [Column(TypeName = "decimal(32, 16)")]
    public decimal Quantity { get; set; }

    public int StatusExtraHour { get; set; }

    public DateTime CalcPayrollDate { get; set; }

    [StringLength(200)]
    public string? Comment { get; set; }

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
    [InverseProperty("EmployeeExtraHours")]
    public virtual EarningCode EarningCodeRefRec { get; set; } = null!;

    [ForeignKey("EmployeeRefRecID")]
    [InverseProperty("EmployeeExtraHours")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;

    [ForeignKey("PayrollRefRecID")]
    [InverseProperty("EmployeeExtraHours")]
    public virtual Payroll PayrollRefRec { get; set; } = null!;
}
