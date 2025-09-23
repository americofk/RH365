using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class PayCycle : AuditableCompanyEntity
{
   
    public long RecID { get; set; }

    public int ID { get; set; }

    public int PayCycleID { get; set; }

    public long PayrollRefRecID { get; set; }

    public DateTime PeriodStartDate { get; set; }

    public DateTime PeriodEndDate { get; set; }

    public DateTime DefaultPayDate { get; set; }

    public DateTime PayDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountPaidPerPeriod { get; set; }

    public int StatusPeriod { get; set; }

    public bool IsForTax { get; set; }

    public bool IsForTss { get; set; }

    [ForeignKey("PayrollRefRecID")]
    [InverseProperty("PayCycles")]
    public virtual Payroll PayrollRefRec { get; set; } = null!;
}
