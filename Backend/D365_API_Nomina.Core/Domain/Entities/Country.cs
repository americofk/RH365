using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class Country : AuditableCompanyEntity
{
   
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

    [InverseProperty("CountryRefRec")]
    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    [InverseProperty("CountryRefRec")]
    public virtual ICollection<EmployeesAddress> EmployeesAddresses { get; set; } = new List<EmployeesAddress>();
}
