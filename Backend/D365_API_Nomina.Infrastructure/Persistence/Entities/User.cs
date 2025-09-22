using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

[Index("Alias", Name = "UQ_Users_Alias", IsUnique = true)]
public partial class User
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

    [StringLength(500)]
    public string? Observations { get; set; }

    [StringLength(10)]
    public string DataareaID { get; set; } = null!;

    [StringLength(50)]
    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    [StringLength(50)]
    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

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
