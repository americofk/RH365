using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class EmployeeExtraHour : AuditableCompanyEntity
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
