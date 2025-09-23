// ============================================================================
// Archivo: LoginRequest.cs
// Proyecto: D365_API_Nomina.Core
// Ruta: D365_API_Nomina.Core/Application/Contracts/Login/LoginRequest.cs
// Descripción: Modelo de entrada para la operación de login. Incluye validaciones
//              de reglas de negocio (correo no vacío, contraseña requerida).
// ============================================================================

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace D365_API_Nomina.Core.Application.Contracts.Login
{
    /// <summary>
    /// Payload de autenticación para solicitar login o validar existencia.
    /// </summary>
    public class LoginRequest : IValidatableObject
    {
        public string Email { get; set; } = string.Empty;
        public string? Password { get; set; }
        public bool IsValidateUser { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Email))
                yield return new ValidationResult("El correo o nombre de usuario no puede estar vacío", new[] { nameof(Email) });

            if (IsValidateUser && !string.IsNullOrWhiteSpace(Password))
                yield return new ValidationResult("La contraseña no es necesaria para validar al usuario", new[] { nameof(Password) });

            if (!IsValidateUser && string.IsNullOrWhiteSpace(Password))
                yield return new ValidationResult("La contraseña no puede estar vacía", new[] { nameof(Password) });
        }
    }
}
