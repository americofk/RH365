using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class User
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public string Alias { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Name { get; set; } = null!;

    public long? FormatCodeRefRecId { get; set; }

    public int ElevationType { get; set; }

    public long? CompanyDefaultRefRecId { get; set; }

    public string? TemporaryPassword { get; set; }

    public DateTime? DateTemporaryPassword { get; set; }

    public bool IsActive { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<CompaniesAssignedToUser> CompaniesAssignedToUsers { get; set; } = new List<CompaniesAssignedToUser>();

    public virtual Company? CompanyDefaultRefRec { get; set; }

    public virtual FormatCode? FormatCodeRefRec { get; set; }

    public virtual ICollection<MenuAssignedToUser> MenuAssignedToUsers { get; set; } = new List<MenuAssignedToUser>();

    public virtual ICollection<UserImage> UserImages { get; set; } = new List<UserImage>();
}
