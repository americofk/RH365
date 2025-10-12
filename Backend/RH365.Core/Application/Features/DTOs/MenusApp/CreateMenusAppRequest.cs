// ============================================================================
// Archivo: CreateMenusAppRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/MenusApp/CreateMenusAppRequest.cs
// Descripción:
//   - DTO de creación para MenusApp
//   - No incluye ID/RecID (se generan en BD); auditoría la maneja el DbContext
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.MenusApp
{
    /// <summary>
    /// Payload para crear un menú.
    /// </summary>
    public class CreateMenusAppRequest
    {
        public string MenuCode { get; set; } = null!;
        public string MenuName { get; set; } = null!;
        public string? Description { get; set; }
        public string? Action { get; set; }
        public string Icon { get; set; } = null!;
        public long? MenuFatherRefRecID { get; set; }
        public int Sort { get; set; }
        public bool IsViewMenu { get; set; } = true;
        public string? Observations { get; set; }
    }
}