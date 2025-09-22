using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

[Table("PayrollsProcess")]
public partial class PayrollsProcess : AuditableCompanyEntity
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    [StringLength(20)]
    public string PayrollProcessCode { get; set; } = null!;

    public long? PayrollRefRecID { get; set; }

    [StringLength(200)]
    public string? Description { get; set; }

    public DateTime PaymentDate { get; set; }

    public int EmployeeQuantity { get; set; }

    [StringLength(20)]
    public string? ProjID { get; set; }

    [StringLength(20)]
    public string? ProjCategoryID { get; set; }

    public DateTime PeriodStartDate { get; set; }

    public DateTime PeriodEndDate { get; set; }

    public int PayCycleID { get; set; }

    public int EmployeeQuantityForPay { get; set; }

    public int PayrollProcessStatus { get; set; }

    public bool IsPayCycleTax { get; set; }

    public bool UsedForTax { get; set; }

    public bool IsRoyaltyPayroll { get; set; }

    public bool IsPayCycleTss { get; set; }

    public bool UsedForTss { get; set; }

    public bool IsForHourPayroll { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmountToPay { get; set; }

    [InverseProperty("PayrollProcessRefRec")]
    public virtual ICollection<EmployeeEarningCode> EmployeeEarningCodes { get; set; } = new List<EmployeeEarningCode>();

    [InverseProperty("PayrollProcessRefRec")]
    public virtual ICollection<EmployeeLoanHistory> EmployeeLoanHistories { get; set; } = new List<EmployeeLoanHistory>();

    [InverseProperty("PayrollProcessRefRec")]
    public virtual ICollection<PayrollProcessAction> PayrollProcessActions { get; set; } = new List<PayrollProcessAction>();

    [InverseProperty("PayrollProcessRefRec")]
    public virtual ICollection<PayrollProcessDetail> PayrollProcessDetails { get; set; } = new List<PayrollProcessDetail>();

    [ForeignKey("PayrollRefRecID")]
    [InverseProperty("PayrollsProcesses")]
    public virtual Payroll? PayrollRefRec { get; set; }
}
