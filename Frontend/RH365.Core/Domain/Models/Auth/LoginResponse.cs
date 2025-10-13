// ============================================================================
// Archivo: LoginResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Models/Auth/LoginResponse.cs
// Descripción:
//   - Modelo de respuesta tras login exitoso
//   - Mapea correctamente con la estructura del API backend
// ============================================================================

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Auth
{
    /// <summary>
    /// Response con información de autenticación
    /// </summary>
    public class LoginResponse
    {
        [JsonPropertyName("Token")]
        public string Token { get; set; } = null!;

        [JsonPropertyName("TokenType")]
        public string TokenType { get; set; } = null!;

        [JsonPropertyName("ExpiresIn")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("User")]
        public UserInfo User { get; set; } = null!;
    }

    /// <summary>
    /// Información del usuario autenticado
    /// </summary>
    public class UserInfo
    {
        [JsonPropertyName("Id")]
        public long ID { get; set; }

        [JsonPropertyName("Alias")]
        public string Alias { get; set; } = null!;

        [JsonPropertyName("Name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("Email")]
        public string Email { get; set; } = null!;

        [JsonPropertyName("DefaultCompany")]
        public CompanyInfo DefaultCompany { get; set; } = null!;

        [JsonPropertyName("AuthorizedCompanies")]
        public List<CompanyInfo> AuthorizedCompanies { get; set; } = new();

        [JsonPropertyName("RequiresPasswordChange")]
        public bool RequiresPasswordChange { get; set; }
    }

    /// <summary>
    /// Información de empresa
    /// </summary>
    public class CompanyInfo
    {
        [JsonPropertyName("Id")]
        public string Id { get; set; } = null!;

        [JsonPropertyName("Code")]
        public string Code { get; set; } = null!;

        [JsonPropertyName("Name")]
        public string Name { get; set; } = null!;
    }
}