// ============================================================================
// Archivo: CountryDto.cs
// Capa: RH365.Core.Application
// Ruta: Application/Features/Countries/DTOs/CountryDto.cs
// Descripción: DTO de lectura para País (usa RecID como PK).
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.Country
{
    /// <summary>DTO para respuesta de países.</summary>
    public sealed class CountryDto
    {
        /// <summary>Identificador global (PK) basado en secuencia.</summary>
        public long RecID { get; set; }

        /// <summary>Código del país (ej. DO, US).</summary>
        public string CountryCode { get; set; } = null!;

        /// <summary>Nombre del país.</summary>
        public string Name { get; set; } = null!;

        /// <summary>Código de nacionalidad (opcional).</summary>
        public string? NationalityCode { get; set; }

        /// <summary>Nombre de nacionalidad (opcional).</summary>
        public string? NationalityName { get; set; }

        /// <summary>Usuario creador (RecID del usuario autenticado).</summary>
        public string CreatedBy { get; set; } = null!;

        /// <summary>Fecha/hora de creación (UTC).</summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>Usuario que modificó por última vez.</summary>
        public string? ModifiedBy { get; set; }

        /// <summary>Fecha/hora de última modificación (UTC).</summary>
        public DateTime? ModifiedOn { get; set; }
    }
}
