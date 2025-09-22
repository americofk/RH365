using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class ClassRoom
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    [StringLength(20)]
    public string ClassRoomCode { get; set; } = null!;

    [StringLength(50)]
    public string Name { get; set; } = null!;

    public long CourseLocationRefRecID { get; set; }

    public int MaxStudentQty { get; set; }

    [StringLength(100)]
    public string? Comment { get; set; }

    public TimeOnly AvailableTimeStart { get; set; }

    public TimeOnly AvailableTimeEnd { get; set; }

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

    [ForeignKey("CourseLocationRefRecID")]
    [InverseProperty("ClassRooms")]
    public virtual CourseLocation CourseLocationRefRec { get; set; } = null!;

    [InverseProperty("ClassRoomRefRec")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
