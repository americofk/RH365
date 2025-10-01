// ============================================================================
// Archivo: ProjectDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/DTOs/Project/ProjectDto.cs
// Descripción: DTO de salida (lectura) para la entidad Project.
//   - Incluye campos de negocio y auditoría ISO 27001.
//   - Expone el ID legible (propiedad sombra generada en BD).
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.Project
{
    /// <summary>DTO de lectura para proyectos.</summary>
    public class ProjectDto
    {
        // Identificadores
        public long RecID { get; set; }
        public string ID { get; set; } = null!; // Ej: PROJ-00000001 (formateado en BD)

        // Datos de negocio
        public string ProjectCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? LedgerAccount { get; set; }
        public bool ProjectStatus { get; set; }

        // Auditoría / multiempresa (ISO 27001)
        public string DataareaID { get; set; } = null!;
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
