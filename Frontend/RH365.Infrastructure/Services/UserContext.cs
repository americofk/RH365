// ============================================================================
// Archivo: UserContext.cs (REEMPLAZA COMPLETO)
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/UserContext.cs
// Descripción:
//   - Obtiene DataareaID y UserRefRecID del usuario autenticado.
//   - Fuente de datos (en orden): HttpContext.Items -> Session -> Claims.
//   - NO lanza excepción si faltan valores; hace fallback seguro.
//     * DataareaID: "DAT" (o valor de APP_DEFAULT_DATAAREA si existe).
//     * UserRefRecID: 0 (controlable desde el controlador).
//   - IsAuthenticated se mantiene del principal.
// Notas:
//   - Evita el error "DataareaID no disponible en el contexto." que te salió.
//   - En producción, ideal: poblar Session/Claims en el login.
// ============================================================================

using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace RH365.Infrastructure.Services
{
    public interface IUserContext
    {
        bool IsAuthenticated { get; }
        string DataareaID { get; }     // Empresa (puede ser fallback)
        long UserRefRecID { get; }     // RecID del usuario (0 si no disponible)
    }

    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _http;

        // Nombres de claves/claims esperados (ajústalos a tu implementación)
        public const string ClaimDataareaId = "DataareaID";
        public const string ClaimUserRefRecId = "UserRefRecID";
        public const string ItemDataareaId = "DataareaID";
        public const string ItemUserRefRecId = "UserRefRecID";
        public const string SessionDataareaId = "DataareaID";
        public const string SessionUserRefId = "UserRefRecID";

        public UserContext(IHttpContextAccessor http) => _http = http;

        public bool IsAuthenticated => _http.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public string DataareaID
        {
            get
            {
                var ctx = _http.HttpContext;
                if (ctx == null) return "DAT";

                // 1) Items
                if (ctx.Items.TryGetValue(ItemDataareaId, out var item) && item is string da1 && !string.IsNullOrWhiteSpace(da1))
                    return da1;

                // 2) Session
                var da2 = ctx.Session?.GetString(SessionDataareaId);
                if (!string.IsNullOrWhiteSpace(da2)) return da2;

                // 3) Claims
                var da3 = ctx.User?.FindFirst(ClaimDataareaId)?.Value;
                if (!string.IsNullOrWhiteSpace(da3)) return da3;

                // 4) Fallback (evita excepción)
                var envDefault = Environment.GetEnvironmentVariable("APP_DEFAULT_DATAAREA");
                return string.IsNullOrWhiteSpace(envDefault) ? "DAT" : envDefault!;
            }
        }

        public long UserRefRecID
        {
            get
            {
                var ctx = _http.HttpContext;
                if (ctx == null) return 0L;

                // 1) Items
                if (ctx.Items.TryGetValue(ItemUserRefRecId, out var item))
                {
                    if (item is long l && l > 0) return l;
                    if (item is string s && long.TryParse(s, out var ls) && ls > 0) return ls;
                }

                // 2) Session
                var sRef = ctx.Session?.GetString(SessionUserRefId);
                if (long.TryParse(sRef, out var fromSession) && fromSession > 0) return fromSession;

                // 3) Claims
                var claim = ctx.User?.FindFirst(ClaimUserRefRecId)?.Value;
                if (long.TryParse(claim, out var fromClaim) && fromClaim > 0) return fromClaim;

                // 4) Fallback (evita excepción)
                return 0L;
            }
        }
    }
}
