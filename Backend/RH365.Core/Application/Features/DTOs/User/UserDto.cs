// ============================================================================
// Archivo: UserDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/User/UserDto.cs
// Descripción:
//   - DTO de lectura para la entidad User (dbo.Users)
//   - Incluye claves, datos de seguridad y auditoría (ISO 27001)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.User
{
    /// <summary>
    /// DTO de salida para User.
    /// </summary>
    public class UserDto
    {
        // Identificadores
        public long RecID { get; set; }
        public string? ID { get; set; }

        // Datos principales
        public string Alias { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;

        // Datos de seguridad y configuración
        public long? FormatCodeRefRecID { get; set; }
        public int ElevationType { get; set; }
        public long? CompanyDefaultRefRecID { get; set; }
        public string? TemporaryPassword { get; set; }
        public DateTime? DateTemporaryPassword { get; set; }
        public bool IsActive { get; set; }
        public string? Observations { get; set; }

        // Auditoría ISO 27001
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}