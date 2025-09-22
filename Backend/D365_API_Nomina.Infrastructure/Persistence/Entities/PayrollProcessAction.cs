using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class PayrollProcessAction
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    public long PayrollProcessRefRecID { get; set; }

    public long EmployeeRefRecID { get; set; }

    public int PayrollActionType { get; set; }

    [StringLength(100)]
    public string ActionName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal ActionAmount { get; set; }

    public bool ApplyTax { get; set; }

    public bool ApplyTSS { get; set; }

    public bool ApplyRoyaltyPayroll { get; set; }

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

    [ForeignKey("EmployeeRefRecID")]
    [InverseProperty("PayrollProcessActions")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;

    [ForeignKey("PayrollProcessRefRecID")]
    [InverseProperty("PayrollProcessActions")]
    public virtual PayrollsProcess PayrollProcessRefRec { get; set; } = null!;
}
