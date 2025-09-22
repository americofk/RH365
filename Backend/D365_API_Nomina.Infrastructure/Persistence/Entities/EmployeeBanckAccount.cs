using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

[Table("EmployeeBanckAccount")]
public partial class EmployeeBanckAccount
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    public long EmployeeRefRecID { get; set; }

    [StringLength(100)]
    public string BankName { get; set; } = null!;

    public int AccountType { get; set; }

    [StringLength(30)]
    public string AccountNum { get; set; } = null!;

    [StringLength(5)]
    public string? Currency { get; set; }

    public bool IsPrincipal { get; set; }

    [StringLength(200)]
    public string? Comment { get; set; }

    [StringLength(10)]
    public string DataareaID { get; set; } = null!;

    [StringLength(50)]
    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    [StringLength(50)]
    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    [ForeignKey("EmployeeRefRecID")]
    [InverseProperty("EmployeeBanckAccounts")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;
}
