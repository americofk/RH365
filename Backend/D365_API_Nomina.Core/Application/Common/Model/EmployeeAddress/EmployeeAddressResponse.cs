using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.EmployeeAddress
{
    public class EmployeeAddressResponse
    {
        public int InternalId { get; set; }
        public string Street { get; set; }
        public string Home { get; set; }
        public string Sector { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string ProvinceName { get; set; }
        public string Comment { get; set; }
        public bool IsPrincipal { get; set; }
        public string EmployeeId { get; set; }
        public string CountryId { get; set; }
    }
}
