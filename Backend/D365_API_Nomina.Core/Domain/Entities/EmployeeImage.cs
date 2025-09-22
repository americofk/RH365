using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class EmployeeImage : AuditableCompanyEntity
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    public long EmployeeRefRecID { get; set; }

    public byte[]? Image { get; set; }

    [StringLength(4)]
    public string Extension { get; set; } = null!;

    public bool IsPrincipal { get; set; }

    [StringLength(200)]
    public string? Comment { get; set; }

    [ForeignKey("EmployeeRefRecID")]
    [InverseProperty("EmployeeImages")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;
}
