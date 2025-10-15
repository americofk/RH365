// ============================================================================
// Archivo: AuthService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/AuthService.cs
// Descripción:
//   - Servicio para comunicación con API de autenticación
//   - Usa UrlsService centralizado
//   - Manejo de tokens JWT y sesiones
// ============================================================================

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AuthModels = RH365.Core.Domain.Models.Auth;

namespace RH365.Infrastructure.Services
{
    /// <summary>
    /// Implementación del servicio de autenticación
    /// </summary>
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;

        public AuthService(HttpClient httpClient, IUrlsService urlsService)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
        }

        /// <summary>
        /// Autenticar usuario contra el API
        /// </summary>
        public async Task<AuthModels.LoginResponse> LoginAsync(AuthModels.LoginRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                if (!string.IsNullOrEmpty(request.CompanyId))
                {
                    _httpClient.DefaultRequestHeaders.Remove("X-Company-Id");
                    _httpClient.DefaultRequestHeaders.Add("X-Company-Id", request.CompanyId);
                }

                var url = _urlsService.GetUrl("Auth.Login");
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonSerializer.Deserialize<AuthModels.LoginResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return loginResponse ?? throw new Exception("Respuesta inválida del servidor");
                }

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

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var url = _urlsService.GetUrl("Auth.ChangePassword");
                var response = await _httpClient.PostAsync(url, content);

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

                var url = _urlsService.GetUrl("Auth.Me");
                var response = await _httpClient.GetAsync(url);

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

                var url = _urlsService.GetUrl("Auth.TestHash");
                var response = await _httpClient.GetAsync(url);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}