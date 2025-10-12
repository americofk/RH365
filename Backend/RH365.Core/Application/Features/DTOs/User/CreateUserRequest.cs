// ============================================================================
// Archivo: CreateUserRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/User/CreateUserRequest.cs
// Descripción:
//   - DTO de creación para User (dbo.Users)
//   - No incluye ID/RecID (se generan en BD); auditoría la maneja el DbContext
//   - Incluye datos de seguridad y contraseña
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.User
{
    /// <summary>
    /// Payload para crear un usuario.
    /// </summary>
    public class CreateUserRequest
    {
        /// <summary>Alias único del usuario.</summary>
        public string Alias { get; set; } = null!;

        /// <summary>Correo electrónico del usuario.</summary>
        public string Email { get; set; } = null!;

        /// <summary>Hash de la contraseña para autenticación.</summary>
        public string PasswordHash { get; set; } = null!;

        /// <summary>Nombre completo del usuario.</summary>
        public string Name { get; set; } = null!;

        /// <summary>FK al formato preferido del usuario (opcional).</summary>
        public long? FormatCodeRefRecID { get; set; }

        /// <summary>Nivel de privilegios (Elevación de permisos). Default: 0.</summary>
        public int ElevationType { get; set; }

        /// <summary>FK a la empresa predeterminada del usuario (opcional).</summary>
        public long? CompanyDefaultRefRecID { get; set; }

        /// <summary>Contraseña temporal (opcional).</summary>
        public string? TemporaryPassword { get; set; }

        /// <summary>Fecha de asignación de la contraseña temporal (opcional).</summary>
        public DateTime? DateTemporaryPassword { get; set; }

        /// <summary>Indica si el usuario está activo. Default: true.</summary>
        public bool IsActive { get; set; } = true;

        /// <summary>Observaciones (opcional).</summary>
        public string? Observations { get; set; }
    }
}