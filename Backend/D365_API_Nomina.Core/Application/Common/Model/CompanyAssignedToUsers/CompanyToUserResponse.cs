using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.CompanyAssignedToUsers
{
    public class CompanyToUserResponse
    {
        public string CompanyId { get; set; }
        public string Alias { get; set; }
        public string CompanyName { get; set; }
    }
}
