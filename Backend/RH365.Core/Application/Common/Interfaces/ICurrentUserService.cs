// ============================================================================
// Archivo: ICurrentUserService.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Common/Interfaces/ICurrentUserService.cs
// Descripción: Contrato para obtener información del usuario actual.
//   - Provee datos del usuario autenticado
//   - Usado para auditoría automática ISO 27001
//   - Obtiene empresa activa del usuario
// ============================================================================

using System.Collections.Generic;

namespace RH365.Core.Application.Common.Interfaces
{
    /// <summary>
    /// Servicio para obtener información del usuario autenticado actual.
    /// Crítico para cumplimiento ISO 27001 - Trazabilidad.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// ID único del usuario autenticado.
        /// Usado para campos CreatedBy y ModifiedBy.
        /// </summary>
        string? UserId { get; }

        /// <summary>
        /// Nombre completo del usuario para mostrar.
        /// </summary>
        string? UserName { get; }

        /// <summary>
        /// Email del usuario autenticado.
        /// </summary>
        string? UserEmail { get; }

        /// <summary>
        /// ID de la empresa activa del usuario.
        /// Usado para campo DataareaID en entidades multiempresa.
        /// ISO 27001: Segregación de datos por empresa.
        /// </summary>
        string? CompanyId { get; }

        /// <summary>
        /// Nombre de la empresa activa.
        /// </summary>
        string? CompanyName { get; }

        /// <summary>
        /// Lista de empresas a las que tiene acceso el usuario.
        /// Permite cambio de contexto sin re-login.
        /// </summary>
        IEnumerable<string>? AuthorizedCompanies { get; }

        /// <summary>
        /// Roles asignados al usuario.
        /// Usado para autorización y permisos.
        /// </summary>
        IEnumerable<string>? Roles { get; }

        /// <summary>
        /// Verifica si el usuario está autenticado.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Verifica si el usuario tiene un rol específico.
        /// </summary>
        /// <param name="role">Nombre del rol a verificar.</param>
        /// <returns>True si el usuario tiene el rol.</returns>
        bool IsInRole(string role);

        /// <summary>
        /// Verifica si el usuario tiene acceso a una empresa específica.
        /// </summary>
        /// <param name="companyId">ID de la empresa a verificar.</param>
        /// <returns>True si el usuario tiene acceso.</returns>
        bool HasAccessToCompany(string companyId);
    }
}