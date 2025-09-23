using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;

namespace D365_API_Nomina.WebUI.Attributes
{
    public class AuthorizeRoleAttribute: ActionFilterAttribute
    {
        public AdminType ElevationTypeRequired { get; set; } = AdminType.User;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //Se verifica si la licencia es válida, antes de valida el rol
            bool IsValidLicense = bool.Parse(context.HttpContext.User.FindFirstValue(ClaimTypes.SerialNumber));
            //bool IsValidLicense = true;
            if (!IsValidLicense)
            {
                context.Result = new JsonResult(new Response<string>
                {
                    Succeeded = false,
                    StatusHttp = (int)HttpStatusCode.Forbidden,
                    Errors = new List<string>() { "La licencia para la compañía asignada no es válida" }
                });

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

                base.OnActionExecuting(context);
            }
            else
            {
                AdminType ElevationType = (AdminType)Enum.Parse(typeof(AdminType), context.HttpContext.User.FindFirstValue(ClaimTypes.Actor));

                //La pantalla no necesita rol de admnistrador
                if(ElevationTypeRequired == AdminType.LocalAdmin)
                {
                    if (ElevationType == AdminType.User)
                    {
                        context.Result = new JsonResult(new Response<string>
                        {
                            Succeeded = false,
                            StatusHttp = (int)HttpStatusCode.Unauthorized,
                            Errors = new List<string>() { "Permisos del usuario insuficientes para ejecutar esta acción" }
                        });

                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                }

                base.OnActionExecuting(context);
            }
        }
    }
}
