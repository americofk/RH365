// ============================================================================
// Archivo: CurrentUserInformation.cs
// Proyecto: D365_API_Nomina.WEBUI
// Ruta: D365_API_Nomina.WEBUI/Services/CurrentUserInformation.cs
// Descripción: Servicio de utilidad para obtener información del usuario
//              autenticado desde los Claims del JWT/Contexto HTTP.
//              Convenciones de claims usadas por este proyecto:
//                - ClaimTypes.NameIdentifier => Alias del usuario
//                - ClaimTypes.Email         => Email del usuario
//                - ClaimTypes.PostalCode    => Company Default (string)
// ============================================================================

using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace D365_API_Nomina.WEBUI.Services
{
    /// <summary>
    /// Contrato del servicio que expone datos del usuario autenticado.
    /// </summary>
    public interface ICurrentUserInformation
    {
        /// <summary>Alias del usuario (NameIdentifier).</summary>
        string Alias { get; }

        /// <summary>Email del usuario.</summary>
        string Email { get; }

        /// <summary>Compañía por defecto (string desde claim PostalCode).</summary>
        string DefaultCompany { get; }

        /// <summary>Indica si hay usuario autenticado.</summary>
        bool IsAuthenticated { get; }
    }

    /// <summary>
    /// Implementación basada en IHttpContextAccessor.
    /// </summary>
    public class CurrentUserInformation : ICurrentUserInformation
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserInformation(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

        public string Alias =>
            User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        public string Email =>
            User?.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

        // En este proyecto usamos ClaimTypes.PostalCode para la compañía por defecto.
        public string DefaultCompany =>
            User?.FindFirst(ClaimTypes.PostalCode)?.Value ?? string.Empty;
    }

    /// <summary>
    /// Extensiones para registrar el servicio en DI.
    /// </summary>
    public static class CurrentUserInformationExtensions
    {
        /// <summary>
        /// Registra IHttpContextAccessor y CurrentUserInformation como Scoped.
        /// </summary>
        public static IServiceCollection AddCurrentUserInformation(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserInformation, CurrentUserInformation>();
            return services;
        }
    }
}
