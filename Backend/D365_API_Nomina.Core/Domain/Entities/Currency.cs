using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class Currency : AuditableCompanyEntity
{
   
    public long RecID { get; set; }

    public int ID { get; set; }

    [StringLength(5)]
    public string CurrencyCode { get; set; } = null!;

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [InverseProperty("CurrencyRefRec")]
    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    [InverseProperty("CurrencyRefRec")]
    public virtual ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();

    [InverseProperty("CurrencyRefRec")]
    public virtual ICollection<Taxis> Taxes { get; set; } = new List<Taxis>();
}
