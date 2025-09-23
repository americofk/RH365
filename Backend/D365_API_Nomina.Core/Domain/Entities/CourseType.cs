using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class CourseType : AuditableCompanyEntity
{
   
    public long RecID { get; set; }

    public int ID { get; set; }

    [StringLength(20)]
    public string CourseTypeCode { get; set; } = null!;

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(200)]
    public string? Description { get; set; }

    [InverseProperty("CourseTypeRefRec")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
