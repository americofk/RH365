// ============================================================================
// Archivo: UpdateCountryRequest.cs
// Capa: RH365.Core.Application
// Ruta: Application/Features/Countries/DTOs/UpdateCountryRequest.cs
// Descripción: DTO de entrada para actualizar País por RecID.
// ============================================================================
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.Countries.DTOs
{
    /// <summary>Payload para actualizar un país existente.</summary>
    public sealed class UpdateCountryRequest
    {
        /// <summary>Código del país (2..10). Se normaliza a MAYÚSCULAS.</summary>
        [Required, StringLength(10, MinimumLength = 2)]
        public string CountryCode { get; set; } = null!;

        /// <summary>Nombre del país (2..255). Se normaliza con Trim().</summary>
        [Required, StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        /// <summary>Código de nacionalidad (máx. 10, opcional).</summary>
        [StringLength(10)]
        public string? NationalityCode { get; set; }

        /// <summary>Nombre de nacionalidad (máx. 255, opcional).</summary>
        [StringLength(255)]
        public string? NationalityName { get; set; }
    }
}
