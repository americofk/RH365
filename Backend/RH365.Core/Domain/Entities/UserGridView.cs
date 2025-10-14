// ============================================================================
// Archivo: UserGridView.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/UserGridView.cs
// Descripción: Entidad de dominio para almacenar las vistas guardadas por usuario
//              (Grid/Kanban/Calendar) con auditoría ISO 27001 y multiempresa.
// Notas:
//  - Hereda de AuditableCompanyEntity (incluye RecID, ID, DataareaID, Observations,
//    CreatedBy/On, ModifiedBy/On, RowVersion).
//  - No define navegaciones para mantener independencia del Core.
//  - Las restricciones (tamaños/column names/checks) se aplican en la Configuration.
// ============================================================================

using System;
using RH365.Core.Domain.Common;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa una vista guardada por un usuario para una entidad específica (p. ej., Projects).
    /// Almacena la configuración completa en formato JSON (columnas, filtros, orden, etc.).
    /// </summary>
    public class UserGridView : AuditableCompanyEntity
    {
        /// <summary>
        /// Referencia (RecID) del usuario propietario de la vista.
        /// </summary>
        public long UserRefRecID { get; set; }

        /// <summary>
        /// Nombre de la entidad de negocio a la que aplica la vista (p. ej., "Projects").
        /// </summary>
        public string EntityName { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de vista: "Grid", "Kanban" o "Calendar".
        /// </summary>
        public string ViewType { get; set; } = "Grid";

        /// <summary>
        /// Ámbito de visibilidad: "Private", "Company", "Role" o "Public".
        /// </summary>
        public string ViewScope { get; set; } = "Private";

        /// <summary>
        /// Referencia (RecID) del rol, requerida si ViewScope = "Role".
        /// </summary>
        public long? RoleRefRecID { get; set; }

        /// <summary>
        /// Nombre legible de la vista (único por usuario+entidad).
        /// </summary>
        public string ViewName { get; set; } = string.Empty;

        /// <summary>
        /// Descripción opcional de la vista.
        /// </summary>
        public string? ViewDescription { get; set; }

        /// <summary>
        /// Indica si esta vista es la predeterminada del usuario para la entidad.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Si está bloqueada, evita que otros usuarios (en ámbitos compartidos) la editen.
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// Configuración completa de la vista en JSON (columnas, filtros, ordenamiento, etc.).
        /// </summary>
        public string ViewConfig { get; set; } = "{}";

        /// <summary>
        /// Versión del esquema del JSON (para migraciones de estructura).
        /// </summary>
        public int SchemaVersion { get; set; } = 1;

        /// <summary>
        /// Hash (SHA-256 en hex, 64 chars) para verificar integridad del JSON (opcional).
        /// </summary>
        public string? Checksum { get; set; }

        /// <summary>
        /// Contador de usos de la vista (para analítica/UX).
        /// </summary>
        public int UsageCount { get; set; }

        /// <summary>
        /// Última fecha/hora de uso de la vista.
        /// </summary>
        public DateTime? LastUsedOn { get; set; }

        /// <summary>
        /// Etiquetas de clasificación/búsqueda (separadas por comas).
        /// </summary>
        public string? Tags { get; set; }
    }
}
