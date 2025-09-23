using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class Loan : AuditableCompanyEntity
{
   
    public long RecID { get; set; }

    public int ID { get; set; }

    [StringLength(20)]
    public string LoanCode { get; set; } = null!;

    [StringLength(50)]
    public string Name { get; set; } = null!;

    public DateTime ValidTo { get; set; }

    public DateTime ValidFrom { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal MultiplyAmount { get; set; }

    [StringLength(30)]
    public string? LedgerAccount { get; set; }

    [StringLength(200)]
    public string? Description { get; set; }

    public int PayFrecuency { get; set; }

    public int IndexBase { get; set; }

    public long? DepartmentRefRecID { get; set; }

    public long? ProjCategoryRefRecID { get; set; }

    public long? ProjectRefRecID { get; set; }

    public bool LoanStatus { get; set; }

    [ForeignKey("DepartmentRefRecID")]
    [InverseProperty("Loans")]
    public virtual Department? DepartmentRefRec { get; set; }

    [InverseProperty("LoanRefRec")]
    public virtual ICollection<EmployeeLoanHistory> EmployeeLoanHistories { get; set; } = new List<EmployeeLoanHistory>();

    [InverseProperty("LoanRefRec")]
    public virtual ICollection<EmployeeLoan> EmployeeLoans { get; set; } = new List<EmployeeLoan>();

    [ForeignKey("ProjCategoryRefRecID")]
    [InverseProperty("Loans")]
    public virtual ProjectCategory? ProjCategoryRefRec { get; set; }

    [ForeignKey("ProjectRefRecID")]
    [InverseProperty("Loans")]
    public virtual Project? ProjectRefRec { get; set; }
}
