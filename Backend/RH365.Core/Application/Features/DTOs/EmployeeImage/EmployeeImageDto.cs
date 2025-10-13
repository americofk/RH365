// ============================================================================
// Archivo: EmployeeImageDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeImage/EmployeeImageDto.cs
// Descripción:
//   - DTO de lectura para la entidad EmployeeImage
//   - Incluye imagen en Base64 para fácil transmisión
//   - Incluye claves, datos y auditoría (ISO 27001)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeImage
{
    /// <summary>
    /// DTO de salida para EmployeeImage.
    /// </summary>
    public class EmployeeImageDto
    {
        /// <summary>Clave primaria del registro.</summary>
        public long RecID { get; set; }

        /// <summary>Identificador legible generado por secuencia.</summary>
        public string? ID { get; set; }

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

        // Auditoría ISO 27001
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}