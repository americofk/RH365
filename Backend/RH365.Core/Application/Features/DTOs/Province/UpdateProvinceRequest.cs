// ============================================================================
// Archivo: UpdateProvinceRequest.cs
// Capa: RH365.Core.Application
// Ruta: Application/Features/DTOs/Province/UpdateProvinceRequest.cs
// Descripción: DTO de entrada para actualizar una provincia por RecID.
// ============================================================================
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.Province
{
    /// <summary>Payload para actualizar una provincia existente.</summary>
    public sealed class UpdateProvinceRequest
    {
        /// <summary>Código de provincia (2..10). Se normaliza a MAYÚSCULAS.</summary>
        [Required, StringLength(10, MinimumLength = 2)]
        public string ProvinceCode { get; set; } = null!;

        /// <summary>Nombre de la provincia (2..255). Se normaliza con Trim().</summary>
        [Required, StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = null!;
    }
}
