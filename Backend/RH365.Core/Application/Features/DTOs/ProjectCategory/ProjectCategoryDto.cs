// ============================================================================
// Archivo: ProjectCategoryDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/DTOs/ProjectCategory/ProjectCategoryDto.cs
// Descripción: DTO de salida (lectura) para la entidad ProjectCategory.
//   - Incluye campos de negocio y auditoría ISO 27001.
//   - Expone el ID legible (propiedad sombra generada en BD).
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.ProjectCategory
{
    /// <summary>DTO de lectura para categorías de proyecto.</summary>
    public class ProjectCategoryDto
    {
        // Identificadores
        public long RecID { get; set; }
        public string ID { get; set; } = null!; // Ej: PCAT-00000001 (generado en BD)

        // Datos de negocio
        public string CategoryName { get; set; } = null!;
        public string? LedgerAccount { get; set; }
        public long ProjectRefRecID { get; set; }
        public bool ProjectCategoryStatus { get; set; }

        // Auditoría / multiempresa (ISO 27001)
        public string DataareaID { get; set; } = null!;
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
