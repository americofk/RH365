using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class MenuAssignedToUser
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    public long UserRefRecID { get; set; }

    public long MenuRefRecID { get; set; }

    public bool PrivilegeView { get; set; }

    public bool PrivilegeEdit { get; set; }

    public bool PrivilegeDelete { get; set; }

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

    [ForeignKey("MenuRefRecID")]
    [InverseProperty("MenuAssignedToUsers")]
    public virtual MenusApp MenuRefRec { get; set; } = null!;

    [ForeignKey("UserRefRecID")]
    [InverseProperty("MenuAssignedToUsers")]
    public virtual User UserRefRec { get; set; } = null!;
}
