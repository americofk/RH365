// ============================================================================
// Archivo: UserContext.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/UserContext.cs
// Descripción:
//   - Obtiene DataareaID y UserRefRecID del usuario autenticado
//   - Usa los nombres correctos de sesión que establece LoginController
// ============================================================================

using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace RH365.Infrastructure.Services
{
    public interface IUserContext
    {
        bool IsAuthenticated { get; }
        string DataareaID { get; }
        long UserRefRecID { get; }
    }

    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _http;

        public UserContext(IHttpContextAccessor http) => _http = http;

        public bool IsAuthenticated => _http.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public string DataareaID
        {
            get
            {
                var ctx = _http.HttpContext;
                if (ctx == null) return "DAT";

                // Primero intentar Items
                if (ctx.Items.TryGetValue("DataareaID", out var item) && item is string da1 && !string.IsNullOrWhiteSpace(da1))
                    return da1;

                // Segundo intentar Session con CompanyId (que es lo que guarda LoginController)
                var companyId = ctx.Session?.GetString("CompanyId");
                if (!string.IsNullOrWhiteSpace(companyId))
                    return companyId;

                // Tercero intentar Claims
                var da3 = ctx.User?.FindFirst("DataareaID")?.Value;
                if (!string.IsNullOrWhiteSpace(da3))
                    return da3;

                // Fallback
                return "DAT";
            }
        }

        public long UserRefRecID
        {
            get
            {
                var ctx = _http.HttpContext;
                if (ctx == null) return 0L;

                // Primero intentar Items
                if (ctx.Items.TryGetValue("UserRefRecID", out var item))
                {
                    if (item is long l && l > 0) return l;
                    if (item is string s && long.TryParse(s, out var ls) && ls > 0) return ls;
                }

                // Segundo intentar Session con UserId (que es lo que guarda LoginController)
                var userId = ctx.Session?.GetString("UserId");
                if (long.TryParse(userId, out var fromSession) && fromSession > 0)
                    return fromSession;

                // Tercero intentar Claims
                var claim = ctx.User?.FindFirst("UserRefRecID")?.Value;
                if (long.TryParse(claim, out var fromClaim) && fromClaim > 0)
                    return fromClaim;

                // Fallback
                return 0L;
            }
        }
    }
}