using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class PositionRequirement : AuditableCompanyEntity
{
   
    public long RecID { get; set; }

    public int ID { get; set; }

    [StringLength(30)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string Detail { get; set; } = null!;

    public long PositionRefRecID { get; set; }

    [ForeignKey("PositionRefRecID")]
    [InverseProperty("PositionRequirements")]
    public virtual Position PositionRefRec { get; set; } = null!;
}
