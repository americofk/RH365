// ============================================================================
// Archivo: CreateCompaniesAssignedToUserRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/CompaniesAssignedToUser/CreateCompaniesAssignedToUserRequest.cs
// Descripción:
//   - DTO de creación para CompaniesAssignedToUser
//   - No incluye ID/RecID (se generan en BD); auditoría la maneja el DbContext
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.CompaniesAssignedToUser
{
    /// <summary>
    /// Payload para crear una asignación de empresa a usuario.
    /// </summary>
    public class CreateCompaniesAssignedToUserRequest
    {
        public long CompanyRefRecID { get; set; }
        public long UserRefRecID { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Observations { get; set; }
    }
}