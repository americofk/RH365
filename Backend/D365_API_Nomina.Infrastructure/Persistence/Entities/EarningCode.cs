using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class EarningCode
{
    [Key]
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

    [ForeignKey("DepartmentRefRecID")]
    [InverseProperty("EarningCodes")]
    public virtual Department? DepartmentRefRec { get; set; }

    [InverseProperty("EarningCodeRefRec")]
    public virtual ICollection<EmployeeEarningCode> EmployeeEarningCodes { get; set; } = new List<EmployeeEarningCode>();

    [InverseProperty("EarningCodeRefRec")]
    public virtual ICollection<EmployeeExtraHour> EmployeeExtraHours { get; set; } = new List<EmployeeExtraHour>();
}
