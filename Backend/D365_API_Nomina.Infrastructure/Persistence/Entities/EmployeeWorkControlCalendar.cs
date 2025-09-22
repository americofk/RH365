using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class EmployeeWorkControlCalendar
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    public long EmployeeRefRecID { get; set; }

    public DateTime CalendarDate { get; set; }

    [StringLength(30)]
    public string CalendarDay { get; set; } = null!;

    public TimeOnly WorkFrom { get; set; }

    public TimeOnly WorkTo { get; set; }

    public TimeOnly BreakWorkFrom { get; set; }

    public TimeOnly BreakWorkTo { get; set; }

    [Column(TypeName = "decimal(32, 16)")]
    public decimal TotalHour { get; set; }

    public int StatusWorkControl { get; set; }

    public long? PayrollProcessRefRecID { get; set; }

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

    [ForeignKey("EmployeeRefRecID")]
    [InverseProperty("EmployeeWorkControlCalendars")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;
}
