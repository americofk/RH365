using D365_API_Nomina.Core.Application.CommandsAndQueries.LicenseValidations;
using D365_API_Nomina.Core.Application.Common.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace D365_API_Nomina.WebUI.Services
{
    public static class APIKeyAuthentication
    {
        public static IApplicationBuilder UseAPIKeyAuthentication(this IApplicationBuilder app)
        {
            app.UseMiddleware<APIKeyAuthenticationMiddleware>();
            return app;
        }

    }

    public class APIKeyAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _Configuration;

        public APIKeyAuthenticationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _Configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var currentKey = _Configuration["AppSettings:Secret-Config"];

            if (context.Request.Path.StartsWithSegments("/api/v2.0/config"))
            {
                if (context.Request.Query.TryGetValue("apikeyvalue", out StringValues receivedkey))
                {
                    if (receivedkey.First().Equals(currentKey))
                        await _next(context);
                    else
                    {
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new Response<string>()
                        {
                            Succeeded = false,
                            StatusHttp = (int)HttpStatusCode.Unauthorized,
                            Errors = new List<string>() { "User not authorizate!" }
                        }));
                    }
                }
                else
                {
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new Response<string>()
                    {
                        Succeeded = false,
                        StatusHttp = (int)HttpStatusCode.Unauthorized,
                        Errors = new List<string>() { "User not authorizate!" }
                    }));
                }
            }
            else
            {
                await _next(context);
            }
        }
    }

  
}
