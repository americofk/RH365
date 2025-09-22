using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.Entities;

public partial class EmployeeTaxis
{
    [Key]
    public long RecID { get; set; }

    public int ID { get; set; }

    public long TaxRefRecID { get; set; }

    public long EmployeeRefRecID { get; set; }

    public DateTime ValidTo { get; set; }

    public DateTime ValidFrom { get; set; }

    public long PayrollRefRecID { get; set; }

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

    [ForeignKey("EmployeeRefRecID")]
    [InverseProperty("EmployeeTaxes")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;

    [ForeignKey("PayrollRefRecID")]
    [InverseProperty("EmployeeTaxes")]
    public virtual Payroll PayrollRefRec { get; set; } = null!;

    [ForeignKey("TaxRefRecID")]
    [InverseProperty("EmployeeTaxes")]
    public virtual Taxis TaxRefRec { get; set; } = null!;
}
