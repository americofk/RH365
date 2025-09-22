using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class Course : AuditableCompanyEntity
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    [StringLength(20)]
    public string CourseCode { get; set; } = null!;

    [StringLength(100)]
    public string Name { get; set; } = null!;

    public long CourseTypeRefRecID { get; set; }

    public long? ClassRoomRefRecID { get; set; }

    [StringLength(200)]
    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsMatrixTraining { get; set; }

    public int InternalExternal { get; set; }

    [StringLength(20)]
    public string? CourseParentId { get; set; }

    public int MinStudents { get; set; }

    public int MaxStudents { get; set; }

    public int Periodicity { get; set; }

    public int QtySessions { get; set; }

    [StringLength(1000)]
    public string Objetives { get; set; } = null!;

    [StringLength(1000)]
    public string Topics { get; set; } = null!;

    public int CourseStatus { get; set; }

    [StringLength(1000)]
    public string? URLDocuments { get; set; }

    [ForeignKey("ClassRoomRefRecID")]
    [InverseProperty("Courses")]
    public virtual ClassRoom? ClassRoomRefRec { get; set; }

    [InverseProperty("CourseRefRec")]
    public virtual ICollection<CourseEmployee> CourseEmployees { get; set; } = new List<CourseEmployee>();

    [InverseProperty("CourseRefRec")]
    public virtual ICollection<CourseInstructor> CourseInstructors { get; set; } = new List<CourseInstructor>();

    [ForeignKey("CourseTypeRefRecID")]
    [InverseProperty("Courses")]
    public virtual CourseType CourseTypeRefRec { get; set; } = null!;
}
