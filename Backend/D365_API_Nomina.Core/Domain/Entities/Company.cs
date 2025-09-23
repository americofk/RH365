using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class Company
    {
        public string CompanyId { get; set; }
        public string Name { get; set; }
        public string Identification { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Responsible { get; set; }

        public string CountryId { get; set; }
        public string CurrencyId { get; set; }

        public string CompanyLogo { get; set; }


        public string LicenseKey { get; set; }


        public bool CompanyStatus { get; set; } = true;
    }
}
