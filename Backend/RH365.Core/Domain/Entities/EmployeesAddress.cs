using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class EmployeesAddress
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public long EmployeeRefRecId { get; set; }

    public string Street { get; set; } = null!;

    public string Home { get; set; } = null!;

    public string Sector { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Province { get; set; } = null!;

    public string? ProvinceName { get; set; }

    public string? Comment { get; set; }

    public bool IsPrincipal { get; set; }

    public long CountryRefRecId { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Country CountryRefRec { get; set; } = null!;

    public virtual Employee EmployeeRefRec { get; set; } = null!;
}
