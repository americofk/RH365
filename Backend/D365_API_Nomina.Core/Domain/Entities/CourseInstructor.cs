using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;


public partial class CourseInstructor : AuditableCompanyEntity
{
    [Key]
    public long CourseRefRecID { get; set; }

    [Key]
    [StringLength(100)]
    public string InstructorName { get; set; } = null!;

    [StringLength(300)]
    public string? Comment { get; set; }

    [ForeignKey("CourseRefRecID")]
    [InverseProperty("CourseInstructors")]
    public virtual Course CourseRefRec { get; set; } = null!;
}
