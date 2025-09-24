using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class EarningCode
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public string EarningCode1 { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsTss { get; set; }

    public bool IsIsr { get; set; }

    public string? ProjId { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    public string? Description { get; set; }

    public int IndexBase { get; set; }

    public decimal MultiplyAmount { get; set; }

    public string? LedgerAccount { get; set; }

    public long? DepartmentRefRecId { get; set; }

    public bool EarningCodeStatus { get; set; }

    public bool IsExtraHours { get; set; }

    public bool IsRoyaltyPayroll { get; set; }

    public bool IsUseDgt { get; set; }

    public bool IsHoliday { get; set; }

    public TimeOnly WorkFrom { get; set; }

    public TimeOnly WorkTo { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Department? DepartmentRefRec { get; set; }

    public virtual ICollection<EmployeeEarningCode> EmployeeEarningCodes { get; set; } = new List<EmployeeEarningCode>();

    public virtual ICollection<EmployeeExtraHour> EmployeeExtraHours { get; set; } = new List<EmployeeExtraHour>();
}
