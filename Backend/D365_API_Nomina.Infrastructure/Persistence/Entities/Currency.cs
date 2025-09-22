using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class Currency
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    [StringLength(5)]
    public string CurrencyCode { get; set; } = null!;

    [StringLength(100)]
    public string Name { get; set; } = null!;

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

    [InverseProperty("CurrencyRefRec")]
    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    [InverseProperty("CurrencyRefRec")]
    public virtual ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();

    [InverseProperty("CurrencyRefRec")]
    public virtual ICollection<Taxis> Taxes { get; set; } = new List<Taxis>();
}
