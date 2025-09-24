using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class EmployeeWorkCalendar
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public long EmployeeRefRecId { get; set; }

    public DateTime CalendarDate { get; set; }

    public string CalendarDay { get; set; } = null!;

    public TimeOnly WorkFrom { get; set; }

    public TimeOnly WorkTo { get; set; }

    public TimeOnly BreakWorkFrom { get; set; }

    public TimeOnly BreakWorkTo { get; set; }

    public decimal TotalHour { get; set; }

    public int StatusWorkControl { get; set; }

    public long? PayrollProcessRefRecId { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Employee EmployeeRefRec { get; set; } = null!;
}
