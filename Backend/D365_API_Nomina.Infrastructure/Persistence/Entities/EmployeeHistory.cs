using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class EmployeeHistory
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    [StringLength(20)]
    public string EmployeeHistoryCode { get; set; } = null!;

    [StringLength(5)]
    public string Type { get; set; } = null!;

    [StringLength(200)]
    public string Description { get; set; } = null!;

    public DateTime RegisterDate { get; set; }

    public long EmployeeRefRecID { get; set; }

    public bool IsUseDGT { get; set; }

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
    [InverseProperty("EmployeeHistories")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;
}
