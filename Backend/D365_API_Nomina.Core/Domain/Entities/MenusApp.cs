using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

[Table("MenusApp")]
public partial class MenusApp : AuditableCompanyEntity
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    [StringLength(20)]
    public string MenuCode { get; set; } = null!;

    [StringLength(50)]
    public string MenuName { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? Action { get; set; }

    [StringLength(100)]
    public string Icon { get; set; } = null!;

    public long? MenuFatherRefRecID { get; set; }

    public int Sort { get; set; }

    public bool IsViewMenu { get; set; }

    [InverseProperty("MenuFatherRefRec")]
    public virtual ICollection<MenusApp> InverseMenuFatherRefRec { get; set; } = new List<MenusApp>();

    [InverseProperty("MenuRefRec")]
    public virtual ICollection<MenuAssignedToUser> MenuAssignedToUsers { get; set; } = new List<MenuAssignedToUser>();

    [ForeignKey("MenuFatherRefRecID")]
    [InverseProperty("InverseMenuFatherRefRec")]
    public virtual MenusApp? MenuFatherRefRec { get; set; }
}
