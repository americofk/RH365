using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class GeneralConfig
{
    public string Id { get; set; } = null!;

    public long RecId { get; set; }

    public string Email { get; set; } = null!;

    public string Smtp { get; set; } = null!;

    public string Smtpport { get; set; } = null!;

    public string EmailPassword { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string DataareaId { get; set; } = null!;

    public string? Observations { get; set; }

    public byte[] RowVersion { get; set; } = null!;
}
