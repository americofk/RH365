using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class PayrollsProcess
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public string PayrollProcessCode { get; set; } = null!;

    public long? PayrollRefRecId { get; set; }

    public string? Description { get; set; }

    public DateTime PaymentDate { get; set; }

    public int EmployeeQuantity { get; set; }

    public string? ProjId { get; set; }

    public string? ProjCategoryId { get; set; }

    public DateTime PeriodStartDate { get; set; }

    public DateTime PeriodEndDate { get; set; }

    public int PayCycleId { get; set; }

    public int EmployeeQuantityForPay { get; set; }

    public int PayrollProcessStatus { get; set; }

    public bool IsPayCycleTax { get; set; }

    public bool UsedForTax { get; set; }

    public bool IsRoyaltyPayroll { get; set; }

    public bool IsPayCycleTss { get; set; }

    public bool UsedForTss { get; set; }

    public bool IsForHourPayroll { get; set; }

    public decimal TotalAmountToPay { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<EmployeeEarningCode> EmployeeEarningCodes { get; set; } = new List<EmployeeEarningCode>();

    public virtual ICollection<EmployeeLoanHistory> EmployeeLoanHistories { get; set; } = new List<EmployeeLoanHistory>();

    public virtual ICollection<PayrollProcessAction> PayrollProcessActions { get; set; } = new List<PayrollProcessAction>();

    public virtual ICollection<PayrollProcessDetail> PayrollProcessDetails { get; set; } = new List<PayrollProcessDetail>();

    public virtual Payroll? PayrollRefRec { get; set; }
}
