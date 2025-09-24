using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class CourseType
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public string CourseTypeCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
