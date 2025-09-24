// ============================================================================
// Archivo: CurrentUserService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/Identity/CurrentUserService.cs
// Descripción: Implementación del servicio de usuario actual.
//   - Lee información del usuario desde HttpContext
//   - Extrae claims JWT para auditoría ISO 27001
//   - Maneja contexto multiempresa
// ============================================================================

using Microsoft.AspNetCore.Http;
using RH365.Core.Application.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace RH365.Infrastructure.Services.Identity
{
    /// <summary>
    /// Servicio que obtiene información del usuario autenticado actual.
    /// Implementa ICurrentUserService para cumplimiento ISO 27001.
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// ID único del usuario autenticado desde JWT claims.
        /// </summary>
        public string? UserId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user?.Identity?.IsAuthenticated != true)
                    return null;

                // Intentar obtener desde diferentes claims posibles
                return user.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                       user.FindFirst("sub")?.Value ??
                       user.FindFirst("userId")?.Value ??
                       user.FindFirst("user_id")?.Value;
            }
        }

        /// <summary>
        /// Nombre completo del usuario desde JWT claims.
        /// </summary>
        public string? UserName
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user?.Identity?.IsAuthenticated != true)
                    return null;

                return user.FindFirst(ClaimTypes.Name)?.Value ??
                       user.FindFirst("name")?.Value ??
                       user.FindFirst("fullName")?.Value ??
                       user.Identity.Name;
            }
        }

        /// <summary>
        /// Email del usuario desde JWT claims.
        /// </summary>
        public string? UserEmail
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user?.Identity?.IsAuthenticated != true)
                    return null;

                return user.FindFirst(ClaimTypes.Email)?.Value ??
                       user.FindFirst("email")?.Value ??
                       user.FindFirst("user_email")?.Value;
            }
        }

        /// <summary>
        /// ID de empresa activa desde JWT claims o header personalizado.
        /// Crítico para multiempresa ISO 27001.
        /// </summary>
        public string? CompanyId
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                if (context == null) return "DAT"; // Default para background jobs

                var user = context.User;

                // Prioridad 1: Header personalizado (permite cambio dinámico)
                if (context.Request.Headers.ContainsKey("X-Company-Id"))
                {
                    var headerValue = context.Request.Headers["X-Company-Id"].ToString();
                    if (!string.IsNullOrEmpty(headerValue))
                        return headerValue;
                }

                // Prioridad 2: JWT Claims
                if (user?.Identity?.IsAuthenticated == true)
                {
                    var companyId = user.FindFirst("companyId")?.Value ??
                                   user.FindFirst("company_id")?.Value ??
                                   user.FindFirst("dataAreaId")?.Value ??
                                   user.FindFirst("tenant")?.Value;

                    if (!string.IsNullOrEmpty(companyId))
                        return companyId;
                }

                // Default: Empresa principal
                return "DAT";
            }
        }

        /// <summary>
        /// Nombre de la empresa activa desde JWT claims.
        /// </summary>
        public string? CompanyName
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user?.Identity?.IsAuthenticated != true)
                    return null;

                return user.FindFirst("companyName")?.Value ??
                       user.FindFirst("company_name")?.Value ??
                       user.FindFirst("tenantName")?.Value;
            }
        }

        /// <summary>
        /// Lista de empresas autorizadas desde JWT claims.
        /// Permite cambio de contexto sin re-login.
        /// </summary>
        public IEnumerable<string>? AuthorizedCompanies
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user?.Identity?.IsAuthenticated != true)
                    return null;

                // Buscar claim que contenga lista de empresas
                var companiesClaimValue = user.FindFirst("authorizedCompanies")?.Value ??
                                         user.FindFirst("companies")?.Value ??
                                         user.FindFirst("tenants")?.Value;

                if (string.IsNullOrEmpty(companiesClaimValue))
                    return new[] { CompanyId ?? "DAT" };

                // Asumir formato: "DAT,EMP2,EMP3" o JSON array
                if (companiesClaimValue.StartsWith('['))
                {
                    // TODO: Deserializar JSON si es necesario
                    return new[] { CompanyId ?? "DAT" };
                }
                else
                {
                    return companiesClaimValue.Split(',')
                        .Where(c => !string.IsNullOrWhiteSpace(c))
                        .Select(c => c.Trim());
                }
            }
        }

        /// <summary>
        /// Roles del usuario desde JWT claims.
        /// </summary>
        public IEnumerable<string>? Roles
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user?.Identity?.IsAuthenticated != true)
                    return null;

                // Obtener todos los claims de rol
                var roleClaims = user.FindAll(ClaimTypes.Role)
                    .Union(user.FindAll("role"))
                    .Union(user.FindAll("roles"))
                    .Select(c => c.Value)
                    .Where(r => !string.IsNullOrWhiteSpace(r))
                    .Distinct();

                return roleClaims.Any() ? roleClaims : null;
            }
        }

        /// <summary>
        /// Verifica si el usuario está autenticado.
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;
            }
        }

        /// <summary>
        /// Verifica si el usuario tiene un rol específico.
        /// </summary>
        /// <param name="role">Nombre del rol a verificar.</param>
        /// <returns>True si el usuario tiene el rol.</returns>
        public bool IsInRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role) || !IsAuthenticated)
                return false;

            var userRoles = Roles;
            if (userRoles == null)
                return false;

            return userRoles.Any(r => string.Equals(r, role, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Verifica si el usuario tiene acceso a una empresa específica.
        /// </summary>
        /// <param name="companyId">ID de la empresa a verificar.</param>
        /// <returns>True si el usuario tiene acceso.</returns>
        public bool HasAccessToCompany(string companyId)
        {
            if (string.IsNullOrWhiteSpace(companyId) || !IsAuthenticated)
                return false;

            var authorizedCompanies = AuthorizedCompanies;
            if (authorizedCompanies == null)
                return false;

            return authorizedCompanies.Any(c => string.Equals(c, companyId, StringComparison.OrdinalIgnoreCase));
        }
    }
}