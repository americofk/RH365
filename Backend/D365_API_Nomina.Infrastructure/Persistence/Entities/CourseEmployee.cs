using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class CourseEmployee
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    public long CourseRefRecID { get; set; }

    public long EmployeeRefRecID { get; set; }

    [StringLength(300)]
    public string? Comment { get; set; }

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

    [ForeignKey("CourseRefRecID")]
    [InverseProperty("CourseEmployees")]
    public virtual Course CourseRefRec { get; set; } = null!;

    [ForeignKey("EmployeeRefRecID")]
    [InverseProperty("CourseEmployees")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;
}
