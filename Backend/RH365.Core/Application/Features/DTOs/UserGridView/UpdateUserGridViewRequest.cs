// ============================================================================
// Archivo: UpdateUserGridViewRequest.cs
// Proyecto: RH365.Core.Application
// Ruta: RH365.Core.Application/DTOs/UserGridViews/UpdateUserGridViewRequest.cs
// Descripción: DTO de entrada para actualizar una vista guardada (PUT/PATCH).
// Notas:
//  - Requiere RecID para localizar el registro.
//  - Concurrencia optimista usando RowVersion (si el controlador la expone).
//  - ModifiedBy/On se establecen por interceptor de auditoría.
// ============================================================================
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.DTOs.UserGridViews
{
    /// <summary>
    /// Solicitud para actualizar una vista guardada existente.
    /// </summary>
    public class UpdateUserGridViewRequest
    {
        /// <summary>Clave técnica a actualizar.</summary>
        [Required]
        public long RecID { get; set; }

        /// <summary>Usuario propietario (permite transferir propiedad si se requiere).</summary>
        [Required]
        public long UserRefRecID { get; set; }

        /// <summary>Entidad de negocio (ej. "Projects").</summary>
        [Required, StringLength(50)]
        public string EntityName { get; set; } = string.Empty;

        /// <summary>Tipo: Grid | Kanban | Calendar.</summary>
        [Required, StringLength(20)]
        [RegularExpression("^(Grid|Kanban|Calendar)$", ErrorMessage = "ViewType debe ser Grid, Kanban o Calendar.")]
        public string ViewType { get; set; } = "Grid";

        /// <summary>Ámbito: Private | Company | Role | Public.</summary>
        [Required, StringLength(20)]
        [RegularExpression("^(Private|Company|Role|Public)$", ErrorMessage = "ViewScope inválido.")]
        public string ViewScope { get; set; } = "Private";

        /// <summary>Rol asociado si ViewScope=Role.</summary>
        public long? RoleRefRecID { get; set; }

        /// <summary>Nombre de la vista.</summary>
        [Required, StringLength(100)]
        public string ViewName { get; set; } = string.Empty;

        /// <summary>Descripción opcional.</summary>
        [StringLength(500)]
        public string? ViewDescription { get; set; }

        /// <summary>Predeterminada.</summary>
        public bool IsDefault { get; set; }

        /// <summary>Bloqueada.</summary>
        public bool IsLocked { get; set; }

        /// <summary>JSON completo de la vista.</summary>
        [Required]
        public string ViewConfig { get; set; } = "{}";

        /// <summary>Versión del esquema del JSON.</summary>
        [Range(1, int.MaxValue)]
        public int SchemaVersion { get; set; } = 1;

        /// <summary>SHA-256 (64 hex) opcional del JSON.</summary>
        [StringLength(64)]
        [RegularExpression("^[A-Fa-f0-9]{64}$", ErrorMessage = "Checksum debe ser SHA-256 (64 hex).")]
        public string? Checksum { get; set; }

        /// <summary>Etiquetas opcionales.</summary>
        [StringLength(200)]
        public string? Tags { get; set; }

        /// <summary>Empresa (multiempresa). Obligatorio.</summary>
        [Required, StringLength(10)]
        public string DataareaID { get; set; } = string.Empty;

        /// <summary>Observaciones de auditoría (opcional).</summary>
        [StringLength(500)]
        public string? Observations { get; set; }

        /// <summary>
        /// Token de concurrencia optimista. Debe mapearse desde/ hacia RowVersion (byte[]).
        /// Sugerencia: exponerlo como Base64 en la API.
        /// </summary>
        public string? ConcurrencyToken { get; set; }
    }
}
