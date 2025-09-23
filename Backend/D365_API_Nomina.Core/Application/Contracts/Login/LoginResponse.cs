// ============================================================================
// Archivo: LoginResponse.cs
// Proyecto: D365_API_Nomina.Core
// Ruta: D365_API_Nomina.Core/Application/Contracts/Login/LoginResponse.cs
// Descripción: Modelo de salida para la operación de login. Devuelve el token
//              JWT y la información básica del usuario autenticado.
// ============================================================================

using System.Collections.Generic;

namespace D365_API_Nomina.Core.Application.Contracts.Login
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Alias { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string FormatCode { get; set; } = string.Empty;
        public string DefaultCompany { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public List<CompanyForUser> UserCompanies { get; set; } = new();
    }

    /// <summary>
    /// Compañía asignada al usuario.
    /// </summary>
    public class CompanyForUser
    {
        public string CompanyId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
    }
}
