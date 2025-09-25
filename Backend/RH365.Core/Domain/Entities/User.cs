// ============================================================================
// Archivo: User.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Security/User.cs
// Descripción: Entidad que representa a los usuarios del sistema.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Contiene credenciales, datos de seguridad y relaciones con empresas
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Core.Domain.Entities;
using System;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un usuario registrado en el sistema.
    /// </summary>
    public class User : AuditableCompanyEntity
    {
        /// <summary>
        /// Alias único del usuario.
        /// </summary>
        public string Alias { get; set; } = null!;

        /// <summary>
        /// Correo electrónico del usuario.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Hash de la contraseña para autenticación.
        /// </summary>
        public string PasswordHash { get; set; } = null!;

        /// <summary>
        /// Nombre completo del usuario.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// FK al formato preferido del usuario.
        /// </summary>
        public long? FormatCodeRefRecID { get; set; }

        /// <summary>
        /// Nivel de privilegios (Elevación de permisos).
        /// </summary>
        public int ElevationType { get; set; }

        /// <summary>
        /// FK a la empresa predeterminada del usuario.
        /// </summary>
        public long? CompanyDefaultRefRecID { get; set; }

        /// <summary>
        /// Contraseña temporal (si se asigna).
        /// </summary>
        public string? TemporaryPassword { get; set; }

        /// <summary>
        /// Fecha de asignación de la contraseña temporal.
        /// </summary>
        public DateTime? DateTemporaryPassword { get; set; }

        /// <summary>
        /// Indica si el usuario está activo.
        /// </summary>
        public bool IsActive { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Empresas asignadas al usuario.
        /// </summary>
        public virtual ICollection<CompaniesAssignedToUser> CompaniesAssignedToUsers { get; set; } = new List<CompaniesAssignedToUser>();

        /// <summary>
        /// Empresa predeterminada asociada.
        /// </summary>
        public virtual Company? CompanyDefaultRefRec { get; set; }

        /// <summary>
        /// Formato de usuario configurado.
        /// </summary>
        public virtual FormatCode? FormatCodeRefRec { get; set; }

        /// <summary>
        /// Menús asignados al usuario.
        /// </summary>
        public virtual ICollection<MenuAssignedToUser> MenuAssignedToUsers { get; set; } = new List<MenuAssignedToUser>();

        /// <summary>
        /// Imágenes asociadas al usuario.
        /// </summary>
        public virtual ICollection<UserImage> UserImages { get; set; } = new List<UserImage>();
    }
}
