using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class Instructor
{
    public string Id { get; set; } = null!;

    public long RecId { get; set; }

    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Mail { get; set; }

    public string Company { get; set; } = null!;

    public string? Comment { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string DataareaId { get; set; } = null!;

    public string? Observations { get; set; }

    public byte[] RowVersion { get; set; } = null!;
}
