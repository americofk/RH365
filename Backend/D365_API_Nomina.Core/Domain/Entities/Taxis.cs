using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class Taxis : AuditableCompanyEntity
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    [StringLength(20)]
    public string TaxCode { get; set; } = null!;

    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(30)]
    public string? LedgerAccount { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    public long CurrencyRefRecID { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal MultiplyAmount { get; set; }

    public int PayFrecuency { get; set; }

    [StringLength(200)]
    public string? Description { get; set; }

    [StringLength(20)]
    public string? LimitPeriod { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal LimitAmount { get; set; }

    public int IndexBase { get; set; }

    public long? ProjectRefRecID { get; set; }

    public long? ProjectCategoryRefRecID { get; set; }

    public long? DepartmentRefRecID { get; set; }

    public bool TaxStatus { get; set; }

    [ForeignKey("CurrencyRefRecID")]
    [InverseProperty("Taxes")]
    public virtual Currency CurrencyRefRec { get; set; } = null!;

    [ForeignKey("DepartmentRefRecID")]
    [InverseProperty("Taxes")]
    public virtual Department? DepartmentRefRec { get; set; }

    [InverseProperty("TaxRefRec")]
    public virtual ICollection<EmployeeTaxis> EmployeeTaxes { get; set; } = new List<EmployeeTaxis>();

    [ForeignKey("ProjectCategoryRefRecID")]
    [InverseProperty("Taxes")]
    public virtual ProjectCategory? ProjectCategoryRefRec { get; set; }

    [ForeignKey("ProjectRefRecID")]
    [InverseProperty("Taxes")]
    public virtual Project? ProjectRefRec { get; set; }

    [InverseProperty("TaxRefRec")]
    public virtual ICollection<TaxDetail> TaxDetails { get; set; } = new List<TaxDetail>();
}
