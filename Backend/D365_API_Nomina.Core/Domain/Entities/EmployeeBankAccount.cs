using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

[Table("EmployeeBanckAccount")]
public partial class EmployeeBankAccount : AuditableCompanyEntity
{
   
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

    [ForeignKey("EmployeeRefRecID")]
    [InverseProperty("EmployeeBanckAccounts")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;
}
