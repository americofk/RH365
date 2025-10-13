// ============================================================================
// Archivo: AuthService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/AuthService.cs
// Descripción:
//   - Servicio para comunicación con API de autenticación
//   - Manejo de tokens JWT y sesiones
// ============================================================================

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AuthModels = RH365.Core.Domain.Models.Auth;

namespace RH365.Infrastructure.Services
{
    /// <summary>
    /// Implementación del servicio de autenticación
    /// </summary>
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;

        public AuthService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _baseUrl = _configuration["ApiSettings:BaseUrl"] ?? "http://localhost:9595/api";
        }

        /// <summary>
        /// Autenticar usuario contra el API
        /// </summary>
        public async Task<AuthModels.LoginResponse> LoginAsync(AuthModels.LoginRequest request)
        {
            try
            {
                // Serializar request
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // Headers para multiempresa
                if (!string.IsNullOrEmpty(request.CompanyId))
                {
                    _httpClient.DefaultRequestHeaders.Remove("X-Company-Id");
                    _httpClient.DefaultRequestHeaders.Add("X-Company-Id", request.CompanyId);
                }

                // Llamar al API
                var response = await _httpClient.PostAsync($"{_baseUrl}/Auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonSerializer.Deserialize<AuthModels.LoginResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return loginResponse ?? throw new Exception("Respuesta inválida del servidor");
                }

                // Manejar errores
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de autenticación: {errorContent}");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("No se pudo conectar con el servidor", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception("Tiempo de espera agotado", ex);
            }
        }

        /// <summary>
        /// Cambiar contraseña
        /// </summary>
        public async Task<bool> ChangePasswordAsync(AuthModels.ChangePasswordRequest request, string token)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // Limpiar headers anteriores y agregar token
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsync($"{_baseUrl}/Auth/change-password", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Obtener información del usuario actual
        /// </summary>
        public async Task<AuthModels.UserInfo> GetCurrentUserAsync(string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"{_baseUrl}/Auth/me");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var userInfo = JsonSerializer.Deserialize<AuthModels.UserInfo>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return userInfo ?? throw new Exception("No se pudo obtener información del usuario");
                }

                throw new Exception("No autorizado");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener usuario: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cerrar sesión (limpiar token en cliente)
        /// </summary>
        public Task LogoutAsync()
        {
            // Limpiar headers
            _httpClient.DefaultRequestHeaders.Authorization = null;
            _httpClient.DefaultRequestHeaders.Remove("X-Company-Id");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Validar si el token es válido
        /// </summary>
        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"{_baseUrl}/Auth/test-hash");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}