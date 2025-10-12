// ============================================================================
// Archivo: CreateUserImageRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/UserImage/CreateUserImageRequest.cs
// Descripción:
//   - DTO de creación para UserImage
//   - Recibe imagen en Base64
//   - No incluye ID/RecID (se generan en BD); auditoría la maneja el DbContext
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.UserImage
{
    /// <summary>
    /// Payload para crear una imagen de usuario.
    /// </summary>
    public class CreateUserImageRequest
    {
        public long UserRefRecID { get; set; }
        public string? ImageBase64 { get; set; }
        public string Extension { get; set; } = null!;
        public string? Observations { get; set; }
    }
}