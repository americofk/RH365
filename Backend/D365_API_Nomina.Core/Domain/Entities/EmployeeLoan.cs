using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class EmployeeLoan : AuditableCompanyEntity
{
   
    public long RecID { get; set; }

    public int ID { get; set; }

    public long LoanRefRecID { get; set; }

    public long EmployeeRefRecID { get; set; }

    public DateTime ValidTo { get; set; }

    public DateTime ValidFrom { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal LoanAmount { get; set; }

    public int StartPeriodForPaid { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PaidAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PendingAmount { get; set; }

    public long PayrollRefRecID { get; set; }

    public int TotalDues { get; set; }

    public int PendingDues { get; set; }

    public int QtyPeriodForPaid { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountByDues { get; set; }

    [InverseProperty("EmployeeLoanRefRec")]
    public virtual ICollection<EmployeeLoanHistory> EmployeeLoanHistories { get; set; } = new List<EmployeeLoanHistory>();

    [ForeignKey("EmployeeRefRecID")]
    [InverseProperty("EmployeeLoans")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;

    [ForeignKey("LoanRefRecID")]
    [InverseProperty("EmployeeLoans")]
    public virtual Loan LoanRefRec { get; set; } = null!;

    [ForeignKey("PayrollRefRecID")]
    [InverseProperty("EmployeeLoans")]
    public virtual Payroll PayrollRefRec { get; set; } = null!;
}
