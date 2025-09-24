using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class CalendarHoliday
{
    public long RecId { get; set; }

    public int Id { get; set; }

    public DateTime CalendarDate { get; set; }

    public string Description { get; set; } = null!;

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;
}
