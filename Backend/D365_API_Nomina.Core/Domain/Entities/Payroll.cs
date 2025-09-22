using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class Payroll : AuditableCompanyEntity
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    [StringLength(20)]
    public string PayrollCode { get; set; } = null!;

    [StringLength(100)]
    public string Name { get; set; } = null!;

    public int PayFrecuency { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    [StringLength(300)]
    public string? Description { get; set; }

    public bool IsRoyaltyPayroll { get; set; }

    public bool IsForHourPayroll { get; set; }

    public int BankSecuence { get; set; }

    public long CurrencyRefRecID { get; set; }

    public bool PayrollStatus { get; set; }

    [ForeignKey("CurrencyRefRecID")]
    [InverseProperty("Payrolls")]
    public virtual Currency CurrencyRefRec { get; set; } = null!;

    [InverseProperty("PayrollRefRec")]
    public virtual ICollection<EmployeeDeductionCode> EmployeeDeductionCodes { get; set; } = new List<EmployeeDeductionCode>();

    [InverseProperty("PayrollRefRec")]
    public virtual ICollection<EmployeeEarningCode> EmployeeEarningCodes { get; set; } = new List<EmployeeEarningCode>();

    [InverseProperty("PayrollRefRec")]
    public virtual ICollection<EmployeeExtraHour> EmployeeExtraHours { get; set; } = new List<EmployeeExtraHour>();

    [InverseProperty("PayrollRefRec")]
    public virtual ICollection<EmployeeLoanHistory> EmployeeLoanHistories { get; set; } = new List<EmployeeLoanHistory>();

    [InverseProperty("PayrollRefRec")]
    public virtual ICollection<EmployeeLoan> EmployeeLoans { get; set; } = new List<EmployeeLoan>();

    [InverseProperty("PayrollRefRec")]
    public virtual ICollection<EmployeeTaxis> EmployeeTaxes { get; set; } = new List<EmployeeTaxis>();

    [InverseProperty("PayrollRefRec")]
    public virtual ICollection<PayCycle> PayCycles { get; set; } = new List<PayCycle>();

    [InverseProperty("PayrollRefRec")]
    public virtual ICollection<PayrollsProcess> PayrollsProcesses { get; set; } = new List<PayrollsProcess>();
}
