using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.Common.Interface
{
    public interface IConnectThirdServices
    {
        public Task<HttpResponseMessage> CallAsync(string url, object body, HttpMethod method, IFormFile file = null);
    }
}
