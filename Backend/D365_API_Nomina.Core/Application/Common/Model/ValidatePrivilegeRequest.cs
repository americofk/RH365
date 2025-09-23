using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model
{
    public class ValidatePrivilegeRequest
    {
        public string Alias { get; set; }
        public string MenuId { get; set; }
        public bool PrivilegeView { get; set; } = true;
        public bool PrivilegeEdit { get; set; } = false;
        public bool PrivilegeDelete { get; set; } = false;
        public AdminType ElevationType { get; set; }
    }
}
