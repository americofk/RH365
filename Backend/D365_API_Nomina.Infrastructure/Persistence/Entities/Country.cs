using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class Country
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    [StringLength(20)]
    public string CountryCode { get; set; } = null!;

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    public string? NationalityCode { get; set; }

    [StringLength(100)]
    public string? NationalityName { get; set; }

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

    [InverseProperty("CountryRefRec")]
    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    [InverseProperty("CountryRefRec")]
    public virtual ICollection<EmployeesAddress> EmployeesAddresses { get; set; } = new List<EmployeesAddress>();
}
