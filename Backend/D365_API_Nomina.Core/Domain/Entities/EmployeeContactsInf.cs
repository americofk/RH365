using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

[Table("EmployeeContactsInf")]
public partial class EmployeeContactsInf : AuditableCompanyEntity
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    public long EmployeeRefRecID { get; set; }

    public int ContactType { get; set; }

    [StringLength(200)]
    public string ContactValue { get; set; } = null!;

    public bool IsPrincipal { get; set; }

    [StringLength(200)]
    public string? Comment { get; set; }

    [ForeignKey("EmployeeRefRecID")]
    [InverseProperty("EmployeeContactsInfs")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;
}
