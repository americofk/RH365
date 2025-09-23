using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class CalendarHoliday : AuditableCompanyEntity
{
  
    public long RecID { get; set; }

    public int ID { get; set; }

    public DateTime CalendarDate { get; set; }

    [StringLength(100)]
    public string Description { get; set; } = null!;
}
