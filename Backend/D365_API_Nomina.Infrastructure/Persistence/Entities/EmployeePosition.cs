using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class EmployeePosition
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    public long EmployeeRefRecID { get; set; }

    public long PositionRefRecID { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public bool EmployeePositionStatus { get; set; }

    [StringLength(200)]
    public string? Comment { get; set; }

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
    [InverseProperty("EmployeePositions")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;

    [ForeignKey("PositionRefRecID")]
    [InverseProperty("EmployeePositions")]
    public virtual Position PositionRefRec { get; set; } = null!;
}
