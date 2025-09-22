using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class DeductionCode
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    [Column("DeductionCode")]
    [StringLength(20)]
    public string DeductionCode1 { get; set; } = null!;

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(20)]
    public string? ProjID { get; set; }

    [StringLength(100)]
    public string? ProjCategory { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(30)]
    public string? LedgerAccount { get; set; }

    public long? DepartmentRefRecID { get; set; }

    public int PayrollAction { get; set; }

    public int Ctbution_IndexBase { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Ctbution_MultiplyAmount { get; set; }

    public int Ctbution_PayFrecuency { get; set; }

    public int Ctbution_LimitPeriod { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Ctbution_LimitAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Ctbution_LimitAmountToApply { get; set; }

    public int Dduction_IndexBase { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Dduction_MultiplyAmount { get; set; }

    public int Dduction_PayFrecuency { get; set; }

    public int Dduction_LimitPeriod { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Dduction_LimitAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Dduction_LimitAmountToApply { get; set; }

    public bool IsForTaxCalc { get; set; }

    public bool IsForTssCalc { get; set; }

    public bool DeductionStatus { get; set; }

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

    [InverseProperty("DeductionCodeRefRec")]
    public virtual ICollection<EmployeeDeductionCode> EmployeeDeductionCodes { get; set; } = new List<EmployeeDeductionCode>();
}
