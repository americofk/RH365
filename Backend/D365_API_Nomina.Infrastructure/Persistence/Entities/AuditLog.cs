using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

[Table("AuditLog")]
public partial class AuditLog
{
    [StringLength(100)]
    public string EntityName { get; set; } = null!;

    [StringLength(100)]
    public string FieldName { get; set; } = null!;

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    [StringLength(50)]
    public string ChangedBy { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime ChangedAt { get; set; }

    [StringLength(20)]
    public string CreatedBy { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreatedOn { get; set; }

    [StringLength(20)]
    public string? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedOn { get; set; }

    [StringLength(10)]
    public string DataAreaId { get; set; } = null!;

    [Key]
    public long RecId { get; set; }

    public long EntityRefRecId { get; set; }

    public long ID { get; set; }
}
