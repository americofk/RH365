using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Common
{
    public class AuditableEntity
    {
        [MaxLength(10)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        [MaxLength(10)]
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }

    }
}
