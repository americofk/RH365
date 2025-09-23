using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class PayrollProcessAction : AuditableCompanyEntity
{
   
    public long RecID { get; set; }

    public int ID { get; set; }

    public long PayrollProcessRefRecID { get; set; }

    public long EmployeeRefRecID { get; set; }

    public int PayrollActionType { get; set; }

    [StringLength(100)]
    public string ActionName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal ActionAmount { get; set; }

    public bool ApplyTax { get; set; }

    public bool ApplyTSS { get; set; }

    public bool ApplyRoyaltyPayroll { get; set; }

    [ForeignKey("EmployeeRefRecID")]
    [InverseProperty("PayrollProcessActions")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;

    [ForeignKey("PayrollProcessRefRecID")]
    [InverseProperty("PayrollProcessActions")]
    public virtual PayrollsProcess PayrollProcessRefRec { get; set; } = null!;
}
