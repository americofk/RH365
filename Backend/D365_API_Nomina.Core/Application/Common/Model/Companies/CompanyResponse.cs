using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Companies
{
    public class CompanyResponse
    {
        public string CompanyId { get; set; }
        public string Name { get; set; }
        public bool CompanyStatus { get; set; }
    }
}
