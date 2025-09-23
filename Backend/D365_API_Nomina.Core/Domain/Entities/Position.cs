using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class Position : AuditableCompanyEntity
{
   
    public long RecID { get; set; }

    public int ID { get; set; }

    [StringLength(20)]
    public string PositionCode { get; set; } = null!;

    [StringLength(50)]
    public string PositionName { get; set; } = null!;

    public bool IsVacant { get; set; }

    public long DepartmentRefRecID { get; set; }

    public long JobRefRecID { get; set; }

    public long? NotifyPositionRefRecID { get; set; }

    public bool PositionStatus { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [StringLength(200)]
    public string? Description { get; set; }

    [ForeignKey("DepartmentRefRecID")]
    [InverseProperty("Positions")]
    public virtual Department DepartmentRefRec { get; set; } = null!;

    [InverseProperty("PositionRefRec")]
    public virtual ICollection<EmployeePosition> EmployeePositions { get; set; } = new List<EmployeePosition>();

    [InverseProperty("NotifyPositionRefRec")]
    public virtual ICollection<Position> InverseNotifyPositionRefRec { get; set; } = new List<Position>();

    [ForeignKey("JobRefRecID")]
    [InverseProperty("Positions")]
    public virtual Job JobRefRec { get; set; } = null!;

    [ForeignKey("NotifyPositionRefRecID")]
    [InverseProperty("InverseNotifyPositionRefRec")]
    public virtual Position? NotifyPositionRefRec { get; set; }

    [InverseProperty("PositionRefRec")]
    public virtual ICollection<PositionRequirement> PositionRequirements { get; set; } = new List<PositionRequirement>();
}
