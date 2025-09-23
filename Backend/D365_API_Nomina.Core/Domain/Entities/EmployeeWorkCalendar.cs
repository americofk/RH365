using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class EmployeeWorkCalendar : AuditableCompanyEntity
{
   
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

    [ForeignKey("EmployeeRefRecID")]
    [InverseProperty("EmployeeWorkCalendars")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;
}
