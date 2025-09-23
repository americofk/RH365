using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class ClassRoom : AuditableCompanyEntity
{
  
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
   

    [ForeignKey("CourseLocationRefRecID")]
    [InverseProperty("ClassRooms")]
    public virtual CourseLocation CourseLocationRefRec { get; set; } = null!;

    [InverseProperty("ClassRoomRefRec")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
