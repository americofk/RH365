// ============================================================================
// Archivo: PositionRequirementDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/PositionRequirement/PositionRequirementDto.cs
// Descripción:
//   - DTO de lectura para la entidad PositionRequirement (dbo.PositionRequirements)
//   - Incluye claves, datos y auditoría (ISO 27001)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.PositionRequirement
{
    /// <summary>
    /// DTO de salida para PositionRequirement.
    /// </summary>
    public class PositionRequirementDto
    {
        // Identificadores
        public long RecID { get; set; }
        public string? ID { get; set; }

        // Datos de negocio
        public string Name { get; set; } = null!;
        public string Detail { get; set; } = null!;
        public long PositionRefRecID { get; set; }
        public string? Observations { get; set; }

        // Auditoría ISO 27001
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}