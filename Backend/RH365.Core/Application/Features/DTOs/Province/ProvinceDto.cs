// ============================================================================
// Archivo: ProvinceDto.cs
// Capa: RH365.Core.Application
// Ruta: Application/Features/DTOs/Province/ProvinceDto.cs
// Descripción: DTO de lectura para Provinces (usa RecID como PK).
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.Province
{
    /// <summary>DTO para respuesta de provincias.</summary>
    public sealed class ProvinceDto
    {
        /// <summary>Identificador global (PK) basado en secuencia.</summary>
        public long RecID { get; set; }

        /// <summary>Código de la provincia (ej. 'SDE').</summary>
        public string ProvinceCode { get; set; } = null!;

        /// <summary>Nombre de la provincia.</summary>
        public string Name { get; set; } = null!;

        /// <summary>Usuario creador.</summary>
        public string CreatedBy { get; set; } = null!;

        /// <summary>Fecha/hora de creación (UTC).</summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>Usuario que modificó por última vez.</summary>
        public string? ModifiedBy { get; set; }

        /// <summary>Fecha/hora de última modificación (UTC).</summary>
        public DateTime? ModifiedOn { get; set; }
    }
}
