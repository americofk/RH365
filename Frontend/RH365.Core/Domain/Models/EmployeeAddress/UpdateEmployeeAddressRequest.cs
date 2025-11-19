// ============================================================================
// Archivo: UpdateEmployeeAddressRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/EmployeeAddress/UpdateEmployeeAddressRequest.cs
// Descripcion: Request para actualizar una direccion de empleado existente
// Estandar: ISO 27001 - Control de modificaciones de datos sensibles
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.EmployeeAddress
{
    /// <summary>
    /// Modelo de request para actualizar una direccion de empleado existente
    /// ISO 27001: Control de cambios y auditoria de modificaciones
    /// </summary>
    public class UpdateEmployeeAddressRequest
    {
        /// <summary>
        /// RecID del empleado asociado (no modificable en update)
        /// </summary>
        [JsonPropertyName("EmployeeRefRecID")]
        public long? EmployeeRefRecID { get; set; }

        /// <summary>
        /// RecID del pais
        /// </summary>
        [JsonPropertyName("CountryRefRecID")]
        public long? CountryRefRecID { get; set; }

        /// <summary>
        /// Calle de la direccion
        /// </summary>
        [MaxLength(200, ErrorMessage = "La calle no puede exceder 200 caracteres")]
        [JsonPropertyName("Street")]
        public string Street { get; set; }

        /// <summary>
        /// Numero de casa o apartamento
        /// </summary>
        [MaxLength(50, ErrorMessage = "El numero de casa no puede exceder 50 caracteres")]
        [JsonPropertyName("Home")]
        public string Home { get; set; }

        /// <summary>
        /// Sector o barrio
        /// </summary>
        [MaxLength(100, ErrorMessage = "El sector no puede exceder 100 caracteres")]
        [JsonPropertyName("Sector")]
        public string Sector { get; set; }

        /// <summary>
        /// Ciudad
        /// </summary>
        [MaxLength(100, ErrorMessage = "La ciudad no puede exceder 100 caracteres")]
        [JsonPropertyName("City")]
        public string City { get; set; }

        /// <summary>
        /// Codigo de provincia (desde enum)
        /// </summary>
        [JsonPropertyName("Province")]
        public string Province { get; set; }

        /// <summary>
        /// Comentarios adicionales sobre la direccion
        /// </summary>
        [MaxLength(500, ErrorMessage = "Los comentarios no pueden exceder 500 caracteres")]
        [JsonPropertyName("Comment")]
        public string Comment { get; set; }

        /// <summary>
        /// Indica si es la direccion principal del empleado
        /// </summary>
        [JsonPropertyName("IsPrincipal")]
        public bool? IsPrincipal { get; set; }
    }
}