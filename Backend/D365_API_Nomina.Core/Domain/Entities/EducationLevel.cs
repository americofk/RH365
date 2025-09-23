using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class EducationLevel : AuditableCompanyEntity
{
   
    public long RecID { get; set; }

    public int ID { get; set; }

    [StringLength(20)]
    public string EducationLevelCode { get; set; } = null!;

    [StringLength(200)]
    public string? Description { get; set; }
}
