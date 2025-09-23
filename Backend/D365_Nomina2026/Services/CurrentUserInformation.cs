using D365_API_Nomina.Core.Application.Common.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace D365_API_Nomina.WebUI.Services
{
    public class CurrentUserInformation : ICurrentUserInformation
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserInformation(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Alias => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        public string Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
        public string ElevationType => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Actor);
        public string Company => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.PostalCode);
        public bool IsLicenseValid => bool.Parse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.SerialNumber));
    }
}
