// ============================================================================
// Archivo: CreateMenuAssignedToUserRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/MenuAssignedToUser/CreateMenuAssignedToUserRequest.cs
// Descripción:
//   - DTO de creación para MenuAssignedToUser
//   - No incluye ID/RecID (se generan en BD); auditoría la maneja el DbContext
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.MenuAssignedToUser
{
    /// <summary>
    /// Payload para crear una asignación de menú a usuario.
    /// </summary>
    public class CreateMenuAssignedToUserRequest
    {
        public long UserRefRecID { get; set; }
        public long MenuRefRecID { get; set; }
        public bool PrivilegeView { get; set; }
        public bool PrivilegeEdit { get; set; }
        public bool PrivilegeDelete { get; set; }
        public string? Observations { get; set; }
    }
}