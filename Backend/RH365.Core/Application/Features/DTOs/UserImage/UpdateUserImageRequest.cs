// ============================================================================
// Archivo: UpdateUserImageRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/UserImage/UpdateUserImageRequest.cs
// Descripción:
//   - DTO de actualización parcial para UserImage
//   - Todos los campos son opcionales; solo se actualiza lo enviado
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.UserImage
{
    /// <summary>
    /// Payload para actualizar (parcial) una imagen de usuario.
    /// </summary>
    public class UpdateUserImageRequest
    {
        public long? UserRefRecID { get; set; }
        public string? ImageBase64 { get; set; }
        public string? Extension { get; set; }
        public string? Observations { get; set; }
    }
}