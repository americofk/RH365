// ============================================================================
// Archivo: CompaniesAssignedToUserDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/CompaniesAssignedToUser/CompaniesAssignedToUserDto.cs
// Descripción:
//   - DTO de lectura para la entidad CompaniesAssignedToUser
//   - Incluye claves, datos y auditoría (ISO 27001)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.CompaniesAssignedToUser
{
    /// <summary>
    /// DTO de salida para CompaniesAssignedToUser.
    /// </summary>
    public class CompaniesAssignedToUserDto
    {
        public long RecID { get; set; }
        public string? ID { get; set; }
        public long CompanyRefRecID { get; set; }
        public long UserRefRecID { get; set; }
        public bool IsActive { get; set; }
        public string? Observations { get; set; }
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}