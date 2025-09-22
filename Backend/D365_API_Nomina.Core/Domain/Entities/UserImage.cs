using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class UserImage : AuditableCompanyEntity
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    public long UserRefRecID { get; set; }

    public byte[]? Image { get; set; }

    [StringLength(4)]
    public string Extension { get; set; } = null!;

    [ForeignKey("UserRefRecID")]
    [InverseProperty("UserImages")]
    public virtual User UserRefRec { get; set; } = null!;
}
