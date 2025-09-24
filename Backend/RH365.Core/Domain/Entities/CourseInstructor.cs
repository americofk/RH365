using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class CourseInstructor
{
    public long CourseRefRecId { get; set; }

    public string InstructorName { get; set; } = null!;

    public string? Comment { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public string Id { get; set; } = null!;

    public virtual Course CourseRefRec { get; set; } = null!;
}
