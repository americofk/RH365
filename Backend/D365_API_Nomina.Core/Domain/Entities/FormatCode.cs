using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class FormatCode : AuditableCompanyEntity
{
   
    public long RecID { get; set; }

    public int ID { get; set; }

    [Column("FormatCode")]
    [StringLength(5)]
    public string FormatCode1 { get; set; } = null!;

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [InverseProperty("FormatCodeRefRec")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
