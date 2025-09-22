using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class Company : AuditableCompanyEntity
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    [StringLength(4)]
    public string CompanyCode { get; set; } = null!;

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(200)]
    public string Email { get; set; } = null!;

    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(100)]
    public string? Responsible { get; set; }

    public long? CountryRefRecID { get; set; }

    public long? CurrencyRefRecID { get; set; }

    [StringLength(500)]
    public string? CompanyLogo { get; set; }

    [StringLength(500)]
    public string? LicenseKey { get; set; }

    [StringLength(50)]
    public string? Identification { get; set; }

    public bool CompanyStatus { get; set; }

    [InverseProperty("CompanyRefRec")]
    public virtual ICollection<CompaniesAssignedToUser> CompaniesAssignedToUsers { get; set; } = new List<CompaniesAssignedToUser>();

    [ForeignKey("CountryRefRecID")]
    [InverseProperty("Companies")]
    public virtual Country? CountryRefRec { get; set; }

    [ForeignKey("CurrencyRefRecID")]
    [InverseProperty("Companies")]
    public virtual Currency? CurrencyRefRec { get; set; }

    [InverseProperty("CompanyDefaultRefRec")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
