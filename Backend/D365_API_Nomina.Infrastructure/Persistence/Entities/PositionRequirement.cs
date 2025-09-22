using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class PositionRequirement
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    [StringLength(30)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string Detail { get; set; } = null!;

    public long PositionRefRecID { get; set; }

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

    [ForeignKey("PositionRefRecID")]
    [InverseProperty("PositionRequirements")]
    public virtual Position PositionRefRec { get; set; } = null!;
}
