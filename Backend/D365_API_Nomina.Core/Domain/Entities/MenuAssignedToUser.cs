using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class MenuAssignedToUser : AuditableCompanyEntity
{
   
    public long RecID { get; set; }

    public int ID { get; set; }

    public long UserRefRecID { get; set; }

    public long MenuRefRecID { get; set; }

    public bool PrivilegeView { get; set; }

    public bool PrivilegeEdit { get; set; }

    public bool PrivilegeDelete { get; set; }

    [ForeignKey("MenuRefRecID")]
    [InverseProperty("MenuAssignedToUsers")]
    public virtual MenusApp MenuRefRec { get; set; } = null!;

    [ForeignKey("UserRefRecID")]
    [InverseProperty("MenuAssignedToUsers")]
    public virtual User UserRefRec { get; set; } = null!;
}
