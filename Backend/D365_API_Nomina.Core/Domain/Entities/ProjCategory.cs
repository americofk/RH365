using D365_API_Nomina.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class ProjCategory: AuditableCompanyEntity
    {
        public string ProjCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string LedgerAccount { get; set; }
        public bool ProjCategoryStatus { get; set; } = true;
    }
}
