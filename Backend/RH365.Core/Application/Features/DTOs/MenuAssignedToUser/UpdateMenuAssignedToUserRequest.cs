// ============================================================================
// Archivo: UpdateMenuAssignedToUserRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/MenuAssignedToUser/UpdateMenuAssignedToUserRequest.cs
// Descripción:
//   - DTO de actualización parcial para MenuAssignedToUser
//   - Todos los campos son opcionales; solo se actualiza lo enviado
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.MenuAssignedToUser
{
    /// <summary>
    /// Payload para actualizar (parcial) una asignación de menú a usuario.
    /// </summary>
    public class UpdateMenuAssignedToUserRequest
    {
        public long? UserRefRecID { get; set; }
        public long? MenuRefRecID { get; set; }
        public bool? PrivilegeView { get; set; }
        public bool? PrivilegeEdit { get; set; }
        public bool? PrivilegeDelete { get; set; }
        public string? Observations { get; set; }
    }
}