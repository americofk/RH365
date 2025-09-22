using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class CourseEmployee : AuditableCompanyEntity
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    public long CourseRefRecID { get; set; }

    public long EmployeeRefRecID { get; set; }

    [StringLength(300)]
    public string? Comment { get; set; }


    [ForeignKey("CourseRefRecID")]
    [InverseProperty("CourseEmployees")]
    public virtual Course CourseRefRec { get; set; } = null!;

    [ForeignKey("EmployeeRefRecID")]
    [InverseProperty("CourseEmployees")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;
}
