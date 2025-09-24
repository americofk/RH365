using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class ClassRoom
{
    public int RecId { get; set; }

    public string Id { get; set; } = null!;

    public string ClassRoomCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public long CourseLocationRefRecId { get; set; }

    public int MaxStudentQty { get; set; }

    public string? Comment { get; set; }

    public TimeOnly AvailableTimeStart { get; set; }

    public TimeOnly AvailableTimeEnd { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual CourseLocation CourseLocationRefRec { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
