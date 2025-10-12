// ============================================================================
// Archivo: UpdateUserRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/User/UpdateUserRequest.cs
// Descripción:
//   - DTO de actualización parcial para User (dbo.Users)
//   - Todos los campos son opcionales; solo se actualiza lo enviado
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.User
{
    /// <summary>
    /// Payload para actualizar (parcial) un usuario.
    /// </summary>
    public class UpdateUserRequest
    {
        public string? Alias { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Name { get; set; }
        public long? FormatCodeRefRecID { get; set; }
        public int? ElevationType { get; set; }
        public long? CompanyDefaultRefRecID { get; set; }
        public string? TemporaryPassword { get; set; }
        public DateTime? DateTemporaryPassword { get; set; }
        public bool? IsActive { get; set; }
        public string? Observations { get; set; }
    }
}