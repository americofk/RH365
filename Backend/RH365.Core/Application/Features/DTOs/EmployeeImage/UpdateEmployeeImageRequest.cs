// ============================================================================
// Archivo: UpdateEmployeeImageRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeImage/UpdateEmployeeImageRequest.cs
// Descripción:
//   - DTO de actualización parcial para EmployeeImage
//   - Todos los campos son opcionales; solo se actualiza lo enviado
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.EmployeeImage
{
    /// <summary>
    /// Payload para actualizar (parcial) una imagen de empleado.
    /// </summary>
    public class UpdateEmployeeImageRequest
    {
        /// <summary>FK al empleado propietario de la imagen.</summary>
        public long? EmployeeRefRecID { get; set; }

        /// <summary>Imagen en formato Base64.</summary>
        public string? ImageBase64 { get; set; }

        /// <summary>Extensión del archivo (ejemplo: jpg, png).</summary>
        public string? Extension { get; set; }

        /// <summary>Indica si es la imagen principal del empleado.</summary>
        public bool? IsPrincipal { get; set; }

        /// <summary>Comentario adicional sobre la imagen.</summary>
        public string? Comment { get; set; }
    }
}