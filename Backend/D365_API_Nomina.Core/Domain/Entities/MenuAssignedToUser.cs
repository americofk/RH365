using D365_API_Nomina.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class MenuAssignedToUser: AuditableEntity
    {
        public string Alias { get; set; }
        public string MenuId { get; set; }
        public bool PrivilegeView { get; set; } = true;
        public bool PrivilegeEdit { get; set; } = false;
        public bool PrivilegeDelete { get; set; } = false;
    }
}
