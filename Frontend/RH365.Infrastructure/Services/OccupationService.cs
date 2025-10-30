// ============================================================================
// Archivo: OccupationService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/OccupationService.cs
// Descripción:
//   - Servicio para comunicación con API de Ocupaciones
//   - Maneja CRUD completo y exportación de datos
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
using RH365.Core.Domain.Models.Occupation;

namespace RH365.Infrastructure.Services
{
    public interface IOccupationService
    {
        Task<OccupationListResponse> GetAllAsync(int pageNumber = 1, int pageSize = 100, CancellationToken ct = default);
        Task<OccupationResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<OccupationResponse> CreateAsync(CreateOccupationRequest request, CancellationToken ct = default);
        Task<OccupationResponse> UpdateAsync(long recId, UpdateOccupationRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
    }

    public class OccupationService : IOccupationService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<OccupationService> _logger;

        public OccupationService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<OccupationService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todas las ocupaciones paginadas
        /// </summary>
        public async Task<OccupationListResponse> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 100,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Occupations")}?pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<OccupationListResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new OccupationListResponse();
                }

                _logger.LogWarning($"Error obteniendo ocupaciones: {response.StatusCode}");
                return new OccupationListResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ocupaciones");
                throw;
            }
        }

        /// <summary>
        /// Obtener ocupación por RecID
        /// </summary>
        public async Task<OccupationResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Occupations")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<OccupationResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener ocupación {recId}");
                throw;
            }
        }

        /// <summary>
        /// Crear nueva ocupación
        /// </summary>
        public async Task<OccupationResponse> CreateAsync(CreateOccupationRequest request, CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("Occupations");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<OccupationResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al crear ocupación: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear ocupación");
                throw;
            }
        }

        /// <summary>
        /// Actualizar ocupación existente
        /// </summary>
        public async Task<OccupationResponse> UpdateAsync(
            long recId,
            UpdateOccupationRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("Occupations")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<OccupationResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al actualizar ocupación: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar ocupación {recId}");
                throw;
            }
        }

        /// <summary>
        /// Eliminar ocupación
        /// </summary>
        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Occupations")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar ocupación {recId}");
                return false;
            }
        }
    }
}
