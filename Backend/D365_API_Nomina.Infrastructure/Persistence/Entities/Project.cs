using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class Project
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    [StringLength(20)]
    public string ProjectCode { get; set; } = null!;

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(20)]
    public string? LedgerAccount { get; set; }

    public bool ProjectStatus { get; set; }

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

    [InverseProperty("ProjectRefRec")]
    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

    [InverseProperty("ProjectRefRec")]
    public virtual ICollection<ProjectCategory> ProjectCategories { get; set; } = new List<ProjectCategory>();

    [InverseProperty("ProjectRefRec")]
    public virtual ICollection<Taxis> Taxes { get; set; } = new List<Taxis>();
}
