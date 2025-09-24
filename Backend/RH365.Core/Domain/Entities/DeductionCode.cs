using System;
using System.Collections.Generic;

namespace RH365.Infrastructure.TempScaffold;

public partial class DeductionCode
{
    public long RecId { get; set; }

    public string Id { get; set; } = null!;

    public string DeductionCode1 { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? ProjId { get; set; }

    public string? ProjCategory { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    public string? Description { get; set; }

    public string? LedgerAccount { get; set; }

    public long? DepartmentRefRecId { get; set; }

    public int PayrollAction { get; set; }

    public int CtbutionIndexBase { get; set; }

    public decimal CtbutionMultiplyAmount { get; set; }

    public int CtbutionPayFrecuency { get; set; }

    public int CtbutionLimitPeriod { get; set; }

    public decimal CtbutionLimitAmount { get; set; }

    public decimal CtbutionLimitAmountToApply { get; set; }

    public int DductionIndexBase { get; set; }

    public decimal DductionMultiplyAmount { get; set; }

    public int DductionPayFrecuency { get; set; }

    public int DductionLimitPeriod { get; set; }

    public decimal DductionLimitAmount { get; set; }

    public decimal DductionLimitAmountToApply { get; set; }

    public bool IsForTaxCalc { get; set; }

    public bool IsForTssCalc { get; set; }

    public bool DeductionStatus { get; set; }

    public string? Observations { get; set; }

    public string DataareaId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<EmployeeDeductionCode> EmployeeDeductionCodes { get; set; } = new List<EmployeeDeductionCode>();
}
