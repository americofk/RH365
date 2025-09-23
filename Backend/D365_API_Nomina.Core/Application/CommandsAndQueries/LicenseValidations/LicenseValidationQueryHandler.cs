using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.LicenseValidations
{
    public interface ILicenseValidationQueryHandler
    {
        public Task<Response<object>> ValidateLicense(string licensekey);
        public Task<Response<object>> ValidateNroControl(string licensekey, int currentControlNum);
    }

    public class LicenseValidationQueryHandler : ILicenseValidationQueryHandler
    {
        private readonly IConnectThirdServices _ConnectThirdServices;
        private readonly AppSettings _configuration;

        private const string thirdpartyurl = "http://localhost:9191/api/v2.0/licensevalidation";

        public LicenseValidationQueryHandler(IConnectThirdServices connectThirdServices, IOptions<AppSettings> configuration)
        {
            _ConnectThirdServices = connectThirdServices;
            _configuration = configuration.Value;
        }

        public async Task<Response<object>> ValidateLicense(string licensekey)
        {
            Response<object> objectReturn = new Response<object>();
            string endpoint = $"?licensekey={licensekey}&&apikeyvalue={_configuration.SecretConfig}";

            var response = await _ConnectThirdServices.CallAsync(thirdpartyurl + endpoint, null, HttpMethod.Get);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode != HttpStatusCode.ServiceUnavailable)
                {
                    var resulError = JsonConvert.DeserializeObject<Response<bool>>(response.Content.ReadAsStringAsync().Result);
                    objectReturn.StatusHttp = resulError.StatusHttp;
                    objectReturn.Errors = (List<string>)resulError.Errors;
                    objectReturn.Data = false;
                }
                else
                {
                    objectReturn.StatusHttp = 500;
                    objectReturn.Errors = new List<string>() { "El servidor cliente no responde" };
                    objectReturn.Data = false;

                }

                return objectReturn;
            }

            return new Response<object>(true);
        }

        public async Task<Response<object>> ValidateNroControl(string licensekey, int currentControlNum)
        {
            Response<object> objectReturn = new Response<object>();
            string endpoint = $"/controlnum?licensekey={licensekey}&controlnum={currentControlNum}&apikeyvalue={_configuration.SecretConfig}";

            var response = await _ConnectThirdServices.CallAsync(thirdpartyurl + endpoint, null, HttpMethod.Get);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode != HttpStatusCode.ServiceUnavailable)
                {
                    var resulError = JsonConvert.DeserializeObject<Response<bool>>(response.Content.ReadAsStringAsync().Result);
                    objectReturn.StatusHttp = resulError.StatusHttp;
                    objectReturn.Errors = (List<string>)resulError.Errors;
                    objectReturn.Data = false;
                }
                else
                {
                    objectReturn.StatusHttp = 500;
                    objectReturn.Errors = new List<string>() { "El servidor cliente no responde" };
                    objectReturn.Data = false;
                }

                return objectReturn;
            }

            return new Response<object>(true);
        }
    }
}
