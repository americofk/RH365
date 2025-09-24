using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class CourseEmployee
{
    public long RecId { get; set; }

    public int Id { get; set; }

    public long CourseRefRecId { get; set; }

    public long EmployeeRefRecId { get; set; }

    public string? Comment { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Course CourseRefRec { get; set; } = null!;

    public virtual Employee EmployeeRefRec { get; set; } = null!;
}
