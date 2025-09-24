using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class EmployeeTaxis
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public long TaxRefRecId { get; set; }

    public long EmployeeRefRecId { get; set; }

    public DateTime ValidTo { get; set; }

    public DateTime ValidFrom { get; set; }

    public long PayrollRefRecId { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Employee EmployeeRefRec { get; set; } = null!;

    public virtual Payroll PayrollRefRec { get; set; } = null!;

    public virtual Taxis TaxRefRec { get; set; } = null!;
}
