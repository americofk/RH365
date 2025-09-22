using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class Course
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
