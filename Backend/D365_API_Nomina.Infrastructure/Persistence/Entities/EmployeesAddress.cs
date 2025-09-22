using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

[Table("EmployeesAddress")]
public partial class EmployeesAddress
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

    [ForeignKey("CountryRefRecID")]
    [InverseProperty("EmployeesAddresses")]
    public virtual Country CountryRefRec { get; set; } = null!;

    [ForeignKey("EmployeeRefRecID")]
    [InverseProperty("EmployeesAddresses")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;
}
