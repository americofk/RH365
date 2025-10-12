// ============================================================================
// Archivo: MenusAppDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/MenusApp/MenusAppDto.cs
// Descripción:
//   - DTO de lectura para la entidad MenusApp
//   - Incluye claves, datos y auditoría (ISO 27001)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.MenusApp
{
    /// <summary>
    /// DTO de salida para MenusApp.
    /// </summary>
    public class MenusAppDto
    {
        public long RecID { get; set; }
        public string? ID { get; set; }
        public string MenuCode { get; set; } = null!;
        public string MenuName { get; set; } = null!;
        public string? Description { get; set; }
        public string? Action { get; set; }
        public string Icon { get; set; } = null!;
        public long? MenuFatherRefRecID { get; set; }
        public int Sort { get; set; }
        public bool IsViewMenu { get; set; }
        public string? Observations { get; set; }
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}