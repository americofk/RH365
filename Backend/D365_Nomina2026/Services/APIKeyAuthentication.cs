// ============================================================================
// Archivo: APIKeyAuthentication.cs
// Proyecto: D365_API_Nomina.WEBUI
// Ruta: D365_API_Nomina.WEBUI/Services/APIKeyAuthentication.cs
// Descripción: Middleware para proteger rutas de configuración con API Key
//              vía querystring (?apikeyvalue=...). Responde 401 en caso inválido.
// ============================================================================

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace D365_API_Nomina.WEBUI.Services
{
    public static class APIKeyAuthentication
    {
        public static IApplicationBuilder UseAPIKeyAuthentication(this IApplicationBuilder app)
        {
            app.UseMiddleware<APIKeyAuthenticationMiddleware>();
            return app;
        }
    }

    /// <summary>
    /// Middleware que valida la API Key para rutas /api/v2.0/config.
    /// La clave esperada se toma de configuración: AppSettings:Secret-Config.
    /// </summary>
    public class APIKeyAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public APIKeyAuthenticationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Solo aplica a endpoints de configuración (ajusta el prefijo si lo necesitas)
            if (context.Request.Path.StartsWithSegments("/api/v2.0/config"))
            {
                string expectedKey = _configuration["AppSettings:Secret-Config"] ?? string.Empty;

                if (context.Request.Query.TryGetValue("apikeyvalue", out StringValues received) &&
                    received.FirstOrDefault() == expectedKey)
                {
                    await _next(context);
                    return;
                }

                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json; charset=utf-8";
                var payload = new
                {
                    Succeeded = false,
                    StatusHttp = context.Response.StatusCode,
                    Errors = new[] { "User not authorized." }
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
                return;
            }

            await _next(context);
        }
    }
}
