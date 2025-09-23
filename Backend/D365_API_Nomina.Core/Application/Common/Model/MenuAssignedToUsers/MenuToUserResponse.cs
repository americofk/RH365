using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.MenuAssignedToUsers
{
    public class MenuToUserResponse
    {
        public string Alias { get; set; }
        public string MenuId { get; set; }
        public bool PrivilegeView { get; set; }
        public bool PrivilegeEdit { get; set; } 
        public bool PrivilegeDelete { get; set; } 
        public string Description { get; set; }
    }
}
