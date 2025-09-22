using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

[Table("EmployeesAddress")]
public partial class EmployeesAddress : AuditableCompanyEntity
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    public long EmployeeRefRecID { get; set; }

    [StringLength(100)]
    public string Street { get; set; } = null!;

    [StringLength(10)]
    public string Home { get; set; } = null!;

    [StringLength(50)]
    public string Sector { get; set; } = null!;

    [StringLength(50)]
    public string City { get; set; } = null!;

    [StringLength(50)]
    public string Province { get; set; } = null!;

    [StringLength(50)]
    public string? ProvinceName { get; set; }

    [StringLength(200)]
    public string? Comment { get; set; }

    public bool IsPrincipal { get; set; }

    public long CountryRefRecID { get; set; }

    [ForeignKey("CountryRefRecID")]
    [InverseProperty("EmployeesAddresses")]
    public virtual Country CountryRefRec { get; set; } = null!;

    [ForeignKey("EmployeeRefRecID")]
    [InverseProperty("EmployeesAddresses")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;
}
