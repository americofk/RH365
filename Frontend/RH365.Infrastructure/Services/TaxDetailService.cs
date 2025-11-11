// ============================================================================
// Archivo: TaxDetailService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/TaxDetailService.cs
// Descripción:
//   - Servicio para comunicación con API de Detalles de Impuestos
//   - Maneja CRUD completo de TaxDetails
//   - Filtra detalles por TaxRefRecID
// Estándar: ISO 27001 - Gestión segura de comunicaciones API
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RH365.Core.Domain.Models.TaxDetail;

namespace RH365.Infrastructure.Services
{
    public interface ITaxDetailService
    {
        Task<List<TaxDetailResponse>> GetAllAsync(CancellationToken ct = default);
        Task<List<TaxDetailResponse>> GetByTaxIdAsync(long taxRecID, CancellationToken ct = default);
        Task<TaxDetailResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<TaxDetailResponse> CreateAsync(CreateTaxDetailRequest request, CancellationToken ct = default);
        Task<TaxDetailResponse> UpdateAsync(long recId, UpdateTaxDetailRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
    }

    public class TaxDetailService : ITaxDetailService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<TaxDetailService> _logger;

        public TaxDetailService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<TaxDetailService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los detalles de impuestos
        /// </summary>
        public async Task<List<TaxDetailResponse>> GetAllAsync(CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("TaxDetails")}?skip=0&take=1000";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);

                    // Manejar diferentes formatos de respuesta
                    var tempResponse = JsonSerializer.Deserialize<JsonElement>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    List<TaxDetailResponse> result;

                    if (tempResponse.ValueKind == JsonValueKind.Array)
                    {
                        result = JsonSerializer.Deserialize<List<TaxDetailResponse>>(json,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                    else if (tempResponse.TryGetProperty("Data", out var dataProperty) ||
                             tempResponse.TryGetProperty("data", out dataProperty))
                    {
                        result = JsonSerializer.Deserialize<List<TaxDetailResponse>>(dataProperty.GetRawText(),
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                    else
                    {
                        result = new List<TaxDetailResponse>();
                    }

                    return result ?? new List<TaxDetailResponse>();
                }

                _logger.LogWarning($"Error obteniendo detalles de impuestos: {response.StatusCode}");
                return new List<TaxDetailResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener detalles de impuestos");
                throw;
            }
        }

        /// <summary>
        /// Obtener detalles de impuesto filtrados por TaxRefRecID
        /// </summary>
        public async Task<List<TaxDetailResponse>> GetByTaxIdAsync(long taxRecID, CancellationToken ct = default)
        {
            try
            {
                var allDetails = await GetAllAsync(ct);
                return allDetails.Where(d => d.TaxRefRecID == taxRecID).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener detalles del impuesto {taxRecID}");
                throw;
            }
        }

        /// <summary>
        /// Obtener detalle de impuesto por RecID
        /// </summary>
        public async Task<TaxDetailResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("TaxDetails")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<TaxDetailResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener detalle de impuesto {recId}");
                throw;
            }
        }

        /// <summary>
        /// Crear nuevo detalle de impuesto
        /// </summary>
        public async Task<TaxDetailResponse> CreateAsync(CreateTaxDetailRequest request, CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("TaxDetails");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<TaxDetailResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al crear detalle de impuesto: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear detalle de impuesto");
                throw;
            }
        }

        /// <summary>
        /// Actualizar detalle de impuesto existente
        /// </summary>
        public async Task<TaxDetailResponse> UpdateAsync(
            long recId,
            UpdateTaxDetailRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("TaxDetails")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<TaxDetailResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al actualizar detalle de impuesto: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar detalle de impuesto {recId}");
                throw;
            }
        }

        /// <summary>
        /// Eliminar detalle de impuesto
        /// </summary>
        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("TaxDetails")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar detalle de impuesto {recId}");
                return false;
            }
        }
    }
}