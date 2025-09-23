using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class EarningCode : AuditableCompanyEntity
{
   
    public long RecID { get; set; }

    public int ID { get; set; }

    [Column("EarningCode")]
    [StringLength(20)]
    public string EarningCode1 { get; set; } = null!;

    [StringLength(100)]
    public string Name { get; set; } = null!;

    public bool IsTSS { get; set; }

    public bool IsISR { get; set; }

    [StringLength(20)]
    public string? ProjID { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public int IndexBase { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal MultiplyAmount { get; set; }

    [StringLength(30)]
    public string? LedgerAccount { get; set; }

    public long? DepartmentRefRecID { get; set; }

    public bool EarningCodeStatus { get; set; }

    public bool IsExtraHours { get; set; }

    public bool IsRoyaltyPayroll { get; set; }

    public bool IsUseDGT { get; set; }

    public bool IsHoliday { get; set; }

    public TimeOnly WorkFrom { get; set; }

    public TimeOnly WorkTo { get; set; }

    [ForeignKey("DepartmentRefRecID")]
    [InverseProperty("EarningCodes")]
    public virtual Department? DepartmentRefRec { get; set; }

    [InverseProperty("EarningCodeRefRec")]
    public virtual ICollection<EmployeeEarningCode> EmployeeEarningCodes { get; set; } = new List<EmployeeEarningCode>();

    [InverseProperty("EarningCodeRefRec")]
    public virtual ICollection<EmployeeExtraHour> EmployeeExtraHours { get; set; } = new List<EmployeeExtraHour>();
}
