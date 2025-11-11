// ============================================================================
// Archivo: TaxService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/TaxService.cs
// Descripción:
//   - Servicio para comunicación con API de Impuestos
//   - Maneja CRUD completo y exportación de datos
//   - Usa modelos de RH365.Core
// Estándar: ISO 27001 - Gestión segura de comunicaciones API
// ============================================================================

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RH365.Core.Domain.Models.Tax;

namespace RH365.Infrastructure.Services
{
    public interface ITaxService
    {
        Task<TaxListResponse> GetAllAsync(int pageNumber = 1, int pageSize = 100, CancellationToken ct = default);
        Task<TaxResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<TaxResponse> CreateAsync(CreateTaxRequest request, CancellationToken ct = default);
        Task<TaxResponse> UpdateAsync(long recId, UpdateTaxRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
        Task<List<TaxResponse>> GetEnabledAsync(CancellationToken ct = default);
    }

    public class TaxService : ITaxService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<TaxService> _logger;

        public TaxService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<TaxService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los impuestos paginados
        /// </summary>
        public async Task<TaxListResponse> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 100,
            CancellationToken ct = default)
        {
            try
            {
                // Convertir pageNumber y pageSize a skip y take
                int skip = (pageNumber - 1) * pageSize;
                int take = pageSize;
                
                var url = $"{_urlsService.GetUrl("Taxis")}?skip={skip}&take={take}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<TaxListResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new TaxListResponse();
                }

                _logger.LogWarning($"Error obteniendo impuestos: {response.StatusCode}");
                return new TaxListResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener impuestos");
                throw;
            }
        }

        /// <summary>
        /// Obtener impuesto por RecID
        /// </summary>
        public async Task<TaxResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Taxis")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<TaxResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener impuesto {recId}");
                throw;
            }
        }

        /// <summary>
        /// Crear nuevo impuesto
        /// </summary>
        public async Task<TaxResponse> CreateAsync(CreateTaxRequest request, CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("Taxis");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<TaxResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al crear impuesto: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear impuesto");
                throw;
            }
        }

        /// <summary>
        /// Actualizar impuesto existente
        /// </summary>
        public async Task<TaxResponse> UpdateAsync(
            long recId,
            UpdateTaxRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("Taxis")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<TaxResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al actualizar impuesto: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar impuesto {recId}");
                throw;
            }
        }

        /// <summary>
        /// Eliminar impuesto
        /// </summary>
        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Taxis")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar impuesto {recId}");
                return false;
            }
        }

        /// <summary>
        /// Obtener impuestos activos
        /// </summary>
        public async Task<List<TaxResponse>> GetEnabledAsync(CancellationToken ct = default)
        {
            try
            {
                var url = _urlsService.GetUrl("Taxis.Enabled");
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<List<TaxResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<TaxResponse>();
                }

                return new List<TaxResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener impuestos activos");
                return new List<TaxResponse>();
            }
        }
    }
}
