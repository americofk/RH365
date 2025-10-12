// ============================================================================
// Archivo: UserImageDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/UserImage/UserImageDto.cs
// Descripción:
//   - DTO de lectura para la entidad UserImage
//   - Incluye imagen en Base64 para fácil transmisión
//   - Incluye claves, datos y auditoría (ISO 27001)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.UserImage
{
    /// <summary>
    /// DTO de salida para UserImage.
    /// </summary>
    public class UserImageDto
    {
        public long RecID { get; set; }
        public string? ID { get; set; }
        public long UserRefRecID { get; set; }
        public string? ImageBase64 { get; set; }
        public string Extension { get; set; } = null!;
        public string? Observations { get; set; }
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}