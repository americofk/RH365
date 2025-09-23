using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Companies
{
    public class CompanyRequest
    {
        public string CompanyId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Responsible { get; set; }

        public string CountryId { get; set; }
        public string CurrencyId { get; set; }

        public string CompanyLogo { get; set; }


        public string LicenseKey { get; set; }
    }
}
