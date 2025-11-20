// ============================================================================
// Archivo: UpdateEmployeeContactInfoRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/EmployeeContactInfo/UpdateEmployeeContactInfoRequest.cs
// Descripcion: Request para actualizar informacion de contacto de empleado
// Estandar: ISO 27001 - Validacion de datos de entrada
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.EmployeeContactInfo
{
    /// <summary>
    /// Request para actualizar informacion de contacto de empleado
    /// Cumple con estandar ISO 27001 para validacion de datos
    /// </summary>
    public class UpdateEmployeeContactInfoRequest
    {
        /// <summary>
        /// Tipo de contacto
        /// 0 = Celular, 1 = Correo, 2 = Telefono, 3 = Otro
        /// Campo obligatorio
        /// </summary>
        [JsonPropertyName("ContactType")]
        [Required(ErrorMessage = "El tipo de contacto es obligatorio")]
        public int ContactType { get; set; }

        /// <summary>
        /// Valor del contacto (numero, email, etc)
        /// Campo obligatorio, maximo 200 caracteres
        /// </summary>
        [JsonPropertyName("ContactValue")]
        [Required(ErrorMessage = "El valor del contacto es obligatorio")]
        [StringLength(200, ErrorMessage = "El valor del contacto no puede exceder 200 caracteres")]
        public string ContactValue { get; set; }

        /// <summary>
        /// Indica si es el contacto principal
        /// Campo opcional
        /// </summary>
        [JsonPropertyName("IsPrimary")]
        public bool? IsPrimary { get; set; }

        /// <summary>
        /// Comentarios adicionales
        /// Campo opcional, maximo 500 caracteres
        /// </summary>
        [JsonPropertyName("Comments")]
        [StringLength(500, ErrorMessage = "Los comentarios no pueden exceder 500 caracteres")]
        public string Comments { get; set; }
    }
}
