using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class Payroll
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public string PayrollCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int PayFrecuency { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    public string? Description { get; set; }

    public bool IsRoyaltyPayroll { get; set; }

    public bool IsForHourPayroll { get; set; }

    public int BankSecuence { get; set; }

    public long CurrencyRefRecId { get; set; }

    public bool PayrollStatus { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Currency CurrencyRefRec { get; set; } = null!;

    public virtual ICollection<EmployeeDeductionCode> EmployeeDeductionCodes { get; set; } = new List<EmployeeDeductionCode>();

    public virtual ICollection<EmployeeEarningCode> EmployeeEarningCodes { get; set; } = new List<EmployeeEarningCode>();

    public virtual ICollection<EmployeeExtraHour> EmployeeExtraHours { get; set; } = new List<EmployeeExtraHour>();

    public virtual ICollection<EmployeeLoanHistory> EmployeeLoanHistories { get; set; } = new List<EmployeeLoanHistory>();

    public virtual ICollection<EmployeeLoan> EmployeeLoans { get; set; } = new List<EmployeeLoan>();

    public virtual ICollection<EmployeeTaxis> EmployeeTaxes { get; set; } = new List<EmployeeTaxis>();

    public virtual ICollection<PayCycle> PayCycles { get; set; } = new List<PayCycle>();

    public virtual ICollection<PayrollsProcess> PayrollsProcesses { get; set; } = new List<PayrollsProcess>();
}
