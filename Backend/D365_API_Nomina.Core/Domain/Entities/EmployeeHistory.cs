using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class EmployeeHistory : AuditableCompanyEntity
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    [StringLength(20)]
    public string EmployeeHistoryCode { get; set; } = null!;

    [StringLength(5)]
    public string Type { get; set; } = null!;

    [StringLength(200)]
    public string Description { get; set; } = null!;

    public DateTime RegisterDate { get; set; }

    public long EmployeeRefRecID { get; set; }

    public bool IsUseDGT { get; set; }

    [ForeignKey("EmployeeRefRecID")]
    [InverseProperty("EmployeeHistories")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;
}
