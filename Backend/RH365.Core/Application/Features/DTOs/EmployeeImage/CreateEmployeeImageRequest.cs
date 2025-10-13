// ============================================================================
// Archivo: CreateEmployeeImageRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeImage/CreateEmployeeImageRequest.cs
// Descripción:
//   - DTO de creación para EmployeeImage
//   - Recibe imagen en Base64
//   - No incluye ID/RecID (se generan en BD); auditoría la maneja el DbContext
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.EmployeeImage
{
    /// <summary>
    /// Payload para crear una imagen de empleado.
    /// </summary>
    public class CreateEmployeeImageRequest
    {
        /// <summary>FK al empleado propietario de la imagen.</summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>Imagen en formato Base64.</summary>
        public string? ImageBase64 { get; set; }

        /// <summary>Extensión del archivo (ejemplo: jpg, png).</summary>
        public string Extension { get; set; } = null!;

        /// <summary>Indica si es la imagen principal del empleado.</summary>
        public bool IsPrincipal { get; set; }

        /// <summary>Comentario adicional sobre la imagen.</summary>
        public string? Comment { get; set; }
    }
}