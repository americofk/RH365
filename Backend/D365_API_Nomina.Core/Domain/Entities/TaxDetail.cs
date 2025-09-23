using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;


namespace D365_API_Nomina.Core.Domain.Entities;

public partial class TaxDetail : AuditableCompanyEntity
{
   
    public long RecID { get; set; }

    public int ID { get; set; }

    public long TaxRefRecID { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AnnualAmountHigher { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AnnualAmountNotExceed { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Percent { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal FixedAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal ApplicableScale { get; set; }

    [ForeignKey("TaxRefRecID")]
    [InverseProperty("TaxDetails")]
    public virtual Taxis TaxRefRec { get; set; } = null!;
}
