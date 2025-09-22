using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class PayCycle
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    public int PayCycleID { get; set; }

    public long PayrollRefRecID { get; set; }

    public DateTime PeriodStartDate { get; set; }

    public DateTime PeriodEndDate { get; set; }

    public DateTime DefaultPayDate { get; set; }

    public DateTime PayDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountPaidPerPeriod { get; set; }

    public int StatusPeriod { get; set; }

    public bool IsForTax { get; set; }

    public bool IsForTss { get; set; }

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

    [ForeignKey("PayrollRefRecID")]
    [InverseProperty("PayCycles")]
    public virtual Payroll PayrollRefRec { get; set; } = null!;
}
