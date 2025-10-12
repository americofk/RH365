// ============================================================================
// Archivo: UpdateCompaniesAssignedToUserRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/CompaniesAssignedToUser/UpdateCompaniesAssignedToUserRequest.cs
// Descripción:
//   - DTO de actualización parcial para CompaniesAssignedToUser
//   - Todos los campos son opcionales; solo se actualiza lo enviado
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.CompaniesAssignedToUser
{
    /// <summary>
    /// Payload para actualizar (parcial) una asignación.
    /// </summary>
    public class UpdateCompaniesAssignedToUserRequest
    {
        public long? CompanyRefRecID { get; set; }
        public long? UserRefRecID { get; set; }
        public bool? IsActive { get; set; }
        public string? Observations { get; set; }
    }
}