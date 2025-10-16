// ============================================================================
// Archivo: UserGridViewsService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/UserGridViewsService.cs
// Descripción:
//   - Servicio para gestión de vistas de usuario (UserGridViews)
//   - Maneja CRUD completo y configuración de vistas por defecto
//   - Usa modelos de RH365.Core
// ============================================================================

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RH365.Core.Domain.Models.UserGridViews;

namespace RH365.Infrastructure.Services
{
    public interface IUserGridViewsService
    {
        Task<UserGridViewResponse> GetDefaultViewAsync(string entityName, long userRefRecID, CancellationToken ct = default);
        Task<List<UserGridViewResponse>> GetViewsAsync(string entityName, CancellationToken ct = default);
        Task<UserGridViewResponse> SaveViewAsync(SaveUserGridViewRequest request, CancellationToken ct = default);
        Task<bool> UpdateViewAsync(long recId, UpdateUserGridViewRequest request, CancellationToken ct = default);
        Task<bool> SetDefaultViewAsync(long recId, CancellationToken ct = default);
        Task<bool> DeleteViewAsync(long recId, CancellationToken ct = default);
    }

    public class UserGridViewsService : IUserGridViewsService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<UserGridViewsService> _logger;

        public UserGridViewsService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<UserGridViewsService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener vista por defecto del usuario para una entidad
        /// </summary>
        public async Task<UserGridViewResponse> GetDefaultViewAsync(
            string entityName,
            long userRefRecID,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("UserGridViews")}/{userRefRecID}?entityName={entityName}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogInformation($"No se encontró vista por defecto para {entityName}");
                    return null;
                }

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<UserGridViewResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                _logger.LogWarning($"Error obteniendo vista por defecto: {response.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener vista por defecto para {entityName}");
                return null;
            }
        }

        /// <summary>
        /// Obtener todas las vistas disponibles para una entidad
        /// </summary>
        public async Task<List<UserGridViewResponse>> GetViewsAsync(
            string entityName,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("UserGridViews")}?entityName={entityName}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<UserGridViewListResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result?.Data ?? new List<UserGridViewResponse>();
                }

                _logger.LogWarning($"Error obteniendo vistas para {entityName}: {response.StatusCode}");
                return new List<UserGridViewResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener vistas para {entityName}");
                return new List<UserGridViewResponse>();
            }
        }

        /// <summary>
        /// Guardar nueva vista
        /// </summary>
        public async Task<UserGridViewResponse> SaveViewAsync(
            SaveUserGridViewRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("UserGridViews");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<UserGridViewResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                var errorContent = await response.Content.ReadAsStringAsync(ct);
                throw new Exception($"Error guardando vista: {response.StatusCode} - {errorContent}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar vista");
                throw;
            }
        }

        /// <summary>
        /// Actualizar vista existente
        /// </summary>
        public async Task<bool> UpdateViewAsync(
            long recId,
            UpdateUserGridViewRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("UserGridViews")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync(ct);
                    _logger.LogWarning($"Error actualizando vista {recId}: {response.StatusCode} - {errorContent}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar vista {recId}");
                return false;
            }
        }

        /// <summary>
        /// Establecer vista como predeterminada
        /// </summary>
        public async Task<bool> SetDefaultViewAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("UserGridViews")}/{recId}/set-default";
                var response = await _httpClient.PostAsync(url, null, ct);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync(ct);
                    _logger.LogWarning($"Error estableciendo vista por defecto {recId}: {response.StatusCode} - {errorContent}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al establecer vista por defecto {recId}");
                return false;
            }
        }

        /// <summary>
        /// Eliminar vista
        /// </summary>
        public async Task<bool> DeleteViewAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("UserGridViews")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync(ct);
                    _logger.LogWarning($"Error eliminando vista {recId}: {response.StatusCode} - {errorContent}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar vista {recId}");
                return false;
            }
        }
    }
}