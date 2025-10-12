// ============================================================================
// Archivo: MenuAssignedToUserDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/MenuAssignedToUser/MenuAssignedToUserDto.cs
// Descripción:
//   - DTO de lectura para la entidad MenuAssignedToUser
//   - Incluye claves, privilegios y auditoría (ISO 27001)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.MenuAssignedToUser
{
    /// <summary>
    /// DTO de salida para MenuAssignedToUser.
    /// </summary>
    public class MenuAssignedToUserDto
    {
        public long RecID { get; set; }
        public string? ID { get; set; }
        public long UserRefRecID { get; set; }
        public long MenuRefRecID { get; set; }
        public bool PrivilegeView { get; set; }
        public bool PrivilegeEdit { get; set; }
        public bool PrivilegeDelete { get; set; }
        public string? Observations { get; set; }
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}