using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class EmployeeLoanHistory
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    public long EmployeeLoanRefRecID { get; set; }

    public long LoanRefRecID { get; set; }

    public long EmployeeRefRecID { get; set; }

    public DateTime PeriodStartDate { get; set; }

    public DateTime PeriodEndDate { get; set; }

    public long PayrollRefRecID { get; set; }

    public long? PayrollProcessRefRecID { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal LoanAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PaidAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PendingAmount { get; set; }

    public int TotalDues { get; set; }

    public int PendingDues { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountByDues { get; set; }

    [StringLength(500)]
    public string? Observations { get; set; }

    [StringLength(10)]
    public string DataareaID { get; set; } = null!;

    [StringLength(50)]
    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    [StringLength(50)]
    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    [ForeignKey("EmployeeLoanRefRecID")]
    [InverseProperty("EmployeeLoanHistories")]
    public virtual EmployeeLoan EmployeeLoanRefRec { get; set; } = null!;

    [ForeignKey("EmployeeRefRecID")]
    [InverseProperty("EmployeeLoanHistories")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;

    [ForeignKey("LoanRefRecID")]
    [InverseProperty("EmployeeLoanHistories")]
    public virtual Loan LoanRefRec { get; set; } = null!;

    [ForeignKey("PayrollProcessRefRecID")]
    [InverseProperty("EmployeeLoanHistories")]
    public virtual PayrollsProcess? PayrollProcessRefRec { get; set; }

    [ForeignKey("PayrollRefRecID")]
    [InverseProperty("EmployeeLoanHistories")]
    public virtual Payroll PayrollRefRec { get; set; } = null!;
}
