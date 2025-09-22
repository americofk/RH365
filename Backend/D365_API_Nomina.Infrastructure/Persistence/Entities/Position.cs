using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class Position
{
    [Key]
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

    [StringLength(500)]
    public string? Observations { get; set; }

    [StringLength(10)]
    public string DataareaID { get; set; } = null!;

    [StringLength(50)]
    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    [StringLength(50)]
    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

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
