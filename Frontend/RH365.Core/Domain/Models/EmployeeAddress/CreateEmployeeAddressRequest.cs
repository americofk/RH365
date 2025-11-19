// ============================================================================
// Archivo: CreateEmployeeAddressRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/EmployeeAddress/CreateEmployeeAddressRequest.cs
// Descripcion: Request para crear una direccion de empleado
// Estandar: ISO 27001 - Trazabilidad de datos de empleados
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.EmployeeAddress
{
    /// <summary>
    /// Modelo de request para crear una nueva direccion de empleado
    /// ISO 27001: Datos minimos requeridos para trazabilidad
    /// </summary>
    public class CreateEmployeeAddressRequest
    {
        /// <summary>
        /// RecID del empleado asociado
        /// </summary>
        [Required(ErrorMessage = "El empleado es requerido")]
        [JsonPropertyName("EmployeeRefRecID")]
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// RecID del pais
        /// </summary>
        [Required(ErrorMessage = "El pais es requerido")]
        [JsonPropertyName("CountryRefRecID")]
        public long CountryRefRecID { get; set; }

        /// <summary>
        /// Calle de la direccion
        /// </summary>
        [Required(ErrorMessage = "La calle es requerida")]
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
        [Required(ErrorMessage = "La ciudad es requerida")]
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
        public bool IsPrincipal { get; set; }
    }
}