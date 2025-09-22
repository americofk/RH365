using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class TaxDetail
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    public long TaxRefRecID { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AnnualAmountHigher { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AnnualAmountNotExceed { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Percent { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal FixedAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal ApplicableScale { get; set; }

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

    [ForeignKey("TaxRefRecID")]
    [InverseProperty("TaxDetails")]
    public virtual Taxis TaxRefRec { get; set; } = null!;
}
