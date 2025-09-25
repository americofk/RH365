using System;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities;

public partial class AuditLog
{
    public string EntityName { get; set; } = null!;

    public string FieldName { get; set; } = null!;

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public string ChangedBy { get; set; } = null!;

    public DateTime ChangedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string DataAreaId { get; set; } = null!;

    public long RecId { get; set; }

    public long EntityRefRecId { get; set; }

    public long Id { get; set; }
}
