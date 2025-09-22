using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class EmployeePosition : AuditableCompanyEntity
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    public long EmployeeRefRecID { get; set; }

    public long PositionRefRecID { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public bool EmployeePositionStatus { get; set; }

    [StringLength(200)]
    public string? Comment { get; set; }

    [ForeignKey("EmployeeRefRecID")]
    [InverseProperty("EmployeePositions")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;

    [ForeignKey("PositionRefRecID")]
    [InverseProperty("EmployeePositions")]
    public virtual Position PositionRefRec { get; set; } = null!;
}
