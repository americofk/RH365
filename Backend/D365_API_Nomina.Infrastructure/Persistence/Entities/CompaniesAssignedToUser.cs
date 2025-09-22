using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class CompaniesAssignedToUser
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    public long CompanyRefRecID { get; set; }

    public long UserRefRecID { get; set; }

    public bool IsActive { get; set; }

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

    [ForeignKey("CompanyRefRecID")]
    [InverseProperty("CompaniesAssignedToUsers")]
    public virtual Company CompanyRefRec { get; set; } = null!;

    [ForeignKey("UserRefRecID")]
    [InverseProperty("CompaniesAssignedToUsers")]
    public virtual User UserRefRec { get; set; } = null!;
}
