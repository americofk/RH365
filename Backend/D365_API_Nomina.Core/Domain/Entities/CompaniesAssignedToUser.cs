using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class CompaniesAssignedToUser : AuditableCompanyEntity
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    public long CompanyRefRecID { get; set; }

    public long UserRefRecID { get; set; }

    public bool IsActive { get; set; }

    

    [ForeignKey("CompanyRefRecID")]
    [InverseProperty("CompaniesAssignedToUsers")]
    public virtual Company CompanyRefRec { get; set; } = null!;

    [ForeignKey("UserRefRecID")]
    [InverseProperty("CompaniesAssignedToUsers")]
    public virtual User UserRefRec { get; set; } = null!;
}
