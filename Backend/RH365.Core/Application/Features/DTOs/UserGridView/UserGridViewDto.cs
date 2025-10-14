// ============================================================================
// Archivo: UserGridViewDto.cs
// Proyecto: RH365.Core.Application
// Ruta: RH365.Core.Application/DTOs/UserGridViews/UserGridViewDto.cs
// Descripción: DTO de salida para exponer vistas guardadas (UserGridViews) en la API.
// Notas:
//  - Incluye todos los campos relevantes para lectura (GET).
//  - Mantiene nombres coherentes con la entidad y la tabla SQL.
// ============================================================================
using System;

namespace RH365.Core.Application.DTOs.UserGridViews
{
    /// <summary>
    /// DTO de salida para una vista guardada por un usuario.
    /// </summary>
    public class UserGridViewDto
    {
        /// <summary>Clave técnica (secuencia global).</summary>
        public long RecID { get; set; }

        /// <summary>Identificador legible (ej. UGV00000001).</summary>
        public string ID { get; set; } = string.Empty;

        /// <summary>Usuario propietario (RecID).</summary>
        public long UserRefRecID { get; set; }

        /// <summary>Nombre de la entidad (ej. "Projects").</summary>
        public string EntityName { get; set; } = string.Empty;

        /// <summary>Tipo de vista: Grid | Kanban | Calendar.</summary>
        public string ViewType { get; set; } = "Grid";

        /// <summary>Ámbito: Private | Company | Role | Public.</summary>
        public string ViewScope { get; set; } = "Private";

        /// <summary>Rol asociado si ViewScope=Role.</summary>
        public long? RoleRefRecID { get; set; }

        /// <summary>Nombre de la vista.</summary>
        public string ViewName { get; set; } = string.Empty;

        /// <summary>Descripción opcional.</summary>
        public string? ViewDescription { get; set; }

        /// <summary>Marca si es la vista predeterminada del usuario.</summary>
        public bool IsDefault { get; set; }

        /// <summary>Evita edición por terceros (si aplica).</summary>
        public bool IsLocked { get; set; }

        /// <summary>JSON completo de configuración de la vista.</summary>
        public string ViewConfig { get; set; } = "{}";

        /// <summary>Versión del esquema de ViewConfig.</summary>
        public int SchemaVersion { get; set; } = 1;

        /// <summary>SHA-256 (64 hex) opcional del JSON.</summary>
        public string? Checksum { get; set; }

        /// <summary>Contador de usos.</summary>
        public int UsageCount { get; set; }

        /// <summary>Último uso de la vista.</summary>
        public DateTime? LastUsedOn { get; set; }

        /// <summary>Etiquetas para búsqueda/agrupación.</summary>
        public string? Tags { get; set; }

        // Auditoría (ISO 27001)
        public string DataareaID { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
        public string? Observations { get; set; }
    }
}
