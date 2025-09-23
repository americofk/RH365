using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class EmployeeTaxis : AuditableCompanyEntity
{
   
    public long RecID { get; set; }

    public int ID { get; set; }

    public long TaxRefRecID { get; set; }

    public long EmployeeRefRecID { get; set; }

    public DateTime ValidTo { get; set; }

    public DateTime ValidFrom { get; set; }

    public long PayrollRefRecID { get; set; }

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
