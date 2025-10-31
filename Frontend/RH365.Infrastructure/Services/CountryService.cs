// ============================================================================
// Archivo: CountryService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/CountryService.cs
// Descripción:
//   - Servicio para comunicación con API de Países
//   - Maneja CRUD completo y exportación de datos
//   - Usa modelos de RH365.Core
// Estándar: ISO 27001 - Gestión segura de comunicación con API
// ============================================================================

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RH365.Core.Domain.Models.Country;

namespace RH365.Infrastructure.Services
{
    public interface ICountryService
    {
        Task<CountryListResponse> GetAllAsync(int pageNumber = 1, int pageSize = 100, CancellationToken ct = default);
        Task<CountryResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<CountryResponse> CreateAsync(CreateCountryRequest request, CancellationToken ct = default);
        Task<CountryResponse> UpdateAsync(long recId, UpdateCountryRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
        Task<List<CountryResponse>> GetEnabledAsync(CancellationToken ct = default);
    }

    public class CountryService : ICountryService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<CountryService> _logger;

        public CountryService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<CountryService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los países paginados
        /// </summary>
        public async Task<CountryListResponse> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 100,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Countries")}?pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<CountryListResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new CountryListResponse();
                }

                _logger.LogWarning($"Error obteniendo países: {response.StatusCode}");
                return new CountryListResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener países");
                throw;
            }
        }

        /// <summary>
        /// Obtener país por RecID
        /// </summary>
        public async Task<CountryResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Countries")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<CountryResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener país {recId}");
                throw;
            }
        }

        /// <summary>
        /// Crear nuevo país
        /// </summary>
        public async Task<CountryResponse> CreateAsync(CreateCountryRequest request, CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("Countries");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<CountryResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al crear país: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear país");
                throw;
            }
        }

        /// <summary>
        /// Actualizar país existente
        /// </summary>
        public async Task<CountryResponse> UpdateAsync(
            long recId,
            UpdateCountryRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("Countries")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<CountryResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al actualizar país: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar país {recId}");
                throw;
            }
        }

        /// <summary>
        /// Eliminar país
        /// </summary>
        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Countries")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar país {recId}");
                return false;
            }
        }

        /// <summary>
        /// Obtener países activos
        /// </summary>
        public async Task<List<CountryResponse>> GetEnabledAsync(CancellationToken ct = default)
        {
            try
            {
                var url = _urlsService.GetUrl("Countries.Enabled");
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<List<CountryResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<CountryResponse>();
                }

                return new List<CountryResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener países activos");
                return new List<CountryResponse>();
            }
        }
    }
}
