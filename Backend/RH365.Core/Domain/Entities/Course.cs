using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class Course
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public string CourseCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public long CourseTypeRefRecId { get; set; }

    public int? ClassRoomRefRecId { get; set; }

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public bool IsMatrixTraining { get; set; }

    public int InternalExternal { get; set; }

    public string? CourseParentId { get; set; }

    public int MinStudents { get; set; }

    public int MaxStudents { get; set; }

    public int Periodicity { get; set; }

    public int QtySessions { get; set; }

    public string Objetives { get; set; } = null!;

    public string Topics { get; set; } = null!;

    public int CourseStatus { get; set; }

    public string? Urldocuments { get; set; }

    public virtual ClassRoom? ClassRoomRefRec { get; set; }

    public virtual ICollection<CourseEmployee> CourseEmployees { get; set; } = new List<CourseEmployee>();

    public virtual ICollection<CourseInstructor> CourseInstructors { get; set; } = new List<CourseInstructor>();

    public virtual CourseType CourseTypeRefRec { get; set; } = null!;
}
