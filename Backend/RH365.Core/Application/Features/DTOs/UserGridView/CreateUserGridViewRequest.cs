// ============================================================================
// Archivo: CreateUserGridViewRequest.cs
// Proyecto: RH365.Core.Application
// Ruta: RH365.Core.Application/DTOs/UserGridViews/CreateUserGridViewRequest.cs
// Descripción: DTO de entrada para crear una nueva vista guardada (POST).
// Notas:
//  - RecID/ID/RowVersion se generan en servidor.
//  - CreatedBy/On y Modified* se resuelven por interceptor de auditoría.
//  - DataareaID es obligatorio (multiempresa).
//  - Validaciones básicas con DataAnnotations.
// ============================================================================
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.DTOs.UserGridViews
{
    /// <summary>
    /// Solicitud para crear una vista guardada por usuario.
    /// </summary>
    public class CreateUserGridViewRequest
    {
        /// <summary>Usuario propietario (RecID).</summary>
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

        /// <summary>Nombre de la vista (único por usuario+entidad).</summary>
        [Required, StringLength(100)]
        public string ViewName { get; set; } = string.Empty;

        /// <summary>Descripción opcional.</summary>
        [StringLength(500)]
        public string? ViewDescription { get; set; }

        /// <summary>Marcar como predeterminada.</summary>
        public bool IsDefault { get; set; } = false;

        /// <summary>Marcar como bloqueada (evita edición).</summary>
        public bool IsLocked { get; set; } = false;

        /// <summary>JSON completo de configuración (columnas, filtros, etc.).</summary>
        [Required]
        public string ViewConfig { get; set; } = "{}";

        /// <summary>Versión del esquema del JSON. Default=1.</summary>
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
    }
}
