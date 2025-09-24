using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class Position
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public string PositionCode { get; set; } = null!;

    public string PositionName { get; set; } = null!;

    public bool IsVacant { get; set; }

    public long DepartmentRefRecId { get; set; }

    public long JobRefRecId { get; set; }

    public long? NotifyPositionRefRecId { get; set; }

    public bool PositionStatus { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Description { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Department DepartmentRefRec { get; set; } = null!;

    public virtual ICollection<EmployeePosition> EmployeePositions { get; set; } = new List<EmployeePosition>();

    public virtual ICollection<Position> InverseNotifyPositionRefRec { get; set; } = new List<Position>();

    public virtual Job JobRefRec { get; set; } = null!;

    public virtual Position? NotifyPositionRefRec { get; set; }

    public virtual ICollection<PositionRequirement> PositionRequirements { get; set; } = new List<PositionRequirement>();
}
