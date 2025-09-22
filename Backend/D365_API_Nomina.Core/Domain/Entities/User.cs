using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class User : AuditableCompanyEntity
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    [StringLength(10)]
    public string Alias { get; set; } = null!;

    [StringLength(200)]
    public string Email { get; set; } = null!;

    [StringLength(512)]
    public string PasswordHash { get; set; } = null!;

    [StringLength(100)]
    public string Name { get; set; } = null!;

    public long FormatCodeRefRecID { get; set; }

    public int ElevationType { get; set; }

    public long? CompanyDefaultRefRecID { get; set; }

    [StringLength(512)]
    public string? TemporaryPassword { get; set; }

    public DateTime? DateTemporaryPassword { get; set; }

    public bool IsActive { get; set; }

    [InverseProperty("UserRefRec")]
    public virtual ICollection<CompaniesAssignedToUser> CompaniesAssignedToUsers { get; set; } = new List<CompaniesAssignedToUser>();

    [ForeignKey("CompanyDefaultRefRecID")]
    [InverseProperty("Users")]
    public virtual Company? CompanyDefaultRefRec { get; set; }

    [ForeignKey("FormatCodeRefRecID")]
    [InverseProperty("Users")]
    public virtual FormatCode FormatCodeRefRec { get; set; } = null!;

    [InverseProperty("UserRefRec")]
    public virtual ICollection<MenuAssignedToUser> MenuAssignedToUsers { get; set; } = new List<MenuAssignedToUser>();

    [InverseProperty("UserRefRec")]
    public virtual ICollection<UserImage> UserImages { get; set; } = new List<UserImage>();
}
