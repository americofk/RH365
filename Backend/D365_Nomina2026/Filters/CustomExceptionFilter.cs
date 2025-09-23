using D365_API_Nomina.Core.Application.Common.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace D365_API_Nomina.WebUI.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new JsonResult(new Response<string>()
            {
                Succeeded = false,
                StatusHttp = (int)HttpStatusCode.InternalServerError,
                Errors = new List<string>(){ context.Exception.InnerException != null? 
                    context.Exception.InnerException.Message:
                    context.Exception.Message}
            });

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}
