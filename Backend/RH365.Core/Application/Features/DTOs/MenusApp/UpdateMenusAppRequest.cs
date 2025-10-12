// ============================================================================
// Archivo: UpdateMenusAppRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/MenusApp/UpdateMenusAppRequest.cs
// Descripción:
//   - DTO de actualización parcial para MenusApp
//   - Todos los campos son opcionales; solo se actualiza lo enviado
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.MenusApp
{
    /// <summary>
    /// Payload para actualizar (parcial) un menú.
    /// </summary>
    public class UpdateMenusAppRequest
    {
        public string? MenuCode { get; set; }
        public string? MenuName { get; set; }
        public string? Description { get; set; }
        public string? Action { get; set; }
        public string? Icon { get; set; }
        public long? MenuFatherRefRecID { get; set; }
        public int? Sort { get; set; }
        public bool? IsViewMenu { get; set; }
        public string? Observations { get; set; }
    }
}