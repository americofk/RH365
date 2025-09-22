using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class ProjectCategory : AuditableCompanyEntity
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    [StringLength(100)]
    public string CategoryName { get; set; } = null!;

    [StringLength(20)]
    public string? LedgerAccount { get; set; }

    public long ProjectRefRecID { get; set; }

    public bool ProjectCategoryStatus { get; set; }

    [InverseProperty("ProjCategoryRefRec")]
    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

    [ForeignKey("ProjectRefRecID")]
    [InverseProperty("ProjectCategories")]
    public virtual Project ProjectRefRec { get; set; } = null!;

    [InverseProperty("ProjectCategoryRefRec")]
    public virtual ICollection<Taxis> Taxes { get; set; } = new List<Taxis>();
}
