using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class EmployeeDocument : AuditableCompanyEntity
{
   
    public long RecID { get; set; }

    public int ID { get; set; }

    public long EmployeeRefRecID { get; set; }

    public int DocumentType { get; set; }

    [StringLength(30)]
    public string DocumentNumber { get; set; } = null!;

    public DateTime DueDate { get; set; }

    public byte[]? FileAttach { get; set; }

    public bool IsPrincipal { get; set; }

    [StringLength(200)]
    public string? Comment { get; set; }

    [ForeignKey("EmployeeRefRecID")]
    [InverseProperty("EmployeeDocuments")]
    public virtual Employee EmployeeRefRec { get; set; } = null!;
}
