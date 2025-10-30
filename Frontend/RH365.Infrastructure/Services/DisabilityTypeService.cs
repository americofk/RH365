// ============================================================================
// Archivo: DisabilityTypeService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/DisabilityTypeService.cs
// Descripción:
//   - Servicio para comunicación con API de Tipos de Discapacidad
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
using RH365.Core.Domain.Models.DisabilityType;

namespace RH365.Infrastructure.Services
{
    public interface IDisabilityTypeService
    {
        Task<DisabilityTypeListResponse> GetAllAsync(int pageNumber = 1, int pageSize = 100, CancellationToken ct = default);
        Task<DisabilityTypeResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<DisabilityTypeResponse> CreateAsync(CreateDisabilityTypeRequest request, CancellationToken ct = default);
        Task<DisabilityTypeResponse> UpdateAsync(long recId, UpdateDisabilityTypeRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
        Task<List<DisabilityTypeResponse>> GetEnabledAsync(CancellationToken ct = default);
    }

    public class DisabilityTypeService : IDisabilityTypeService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<DisabilityTypeService> _logger;

        public DisabilityTypeService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<DisabilityTypeService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los tipos de discapacidad paginados
        /// </summary>
        public async Task<DisabilityTypeListResponse> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 100,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("DisabilityTypes")}?pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<DisabilityTypeListResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new DisabilityTypeListResponse();
                }

                _logger.LogWarning($"Error obteniendo tipos de discapacidad: {response.StatusCode}");
                return new DisabilityTypeListResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tipos de discapacidad");
                throw;
            }
        }

        /// <summary>
        /// Obtener tipo de discapacidad por RecID
        /// </summary>
        public async Task<DisabilityTypeResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("DisabilityTypes")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<DisabilityTypeResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener tipo de discapacidad {recId}");
                throw;
            }
        }

        /// <summary>
        /// Crear nuevo tipo de discapacidad
        /// </summary>
        public async Task<DisabilityTypeResponse> CreateAsync(CreateDisabilityTypeRequest request, CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("DisabilityTypes");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<DisabilityTypeResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al crear tipo de discapacidad: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear tipo de discapacidad");
                throw;
            }
        }

        /// <summary>
        /// Actualizar tipo de discapacidad existente
        /// </summary>
        public async Task<DisabilityTypeResponse> UpdateAsync(
            long recId,
            UpdateDisabilityTypeRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("DisabilityTypes")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<DisabilityTypeResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al actualizar tipo de discapacidad: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar tipo de discapacidad {recId}");
                throw;
            }
        }

        /// <summary>
        /// Eliminar tipo de discapacidad
        /// </summary>
        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("DisabilityTypes")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar tipo de discapacidad {recId}");
                return false;
            }
        }

        /// <summary>
        /// Obtener tipos de discapacidad activos
        /// </summary>
        public async Task<List<DisabilityTypeResponse>> GetEnabledAsync(CancellationToken ct = default)
        {
            try
            {
                var url = _urlsService.GetUrl("DisabilityTypes.Enabled");
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<List<DisabilityTypeResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<DisabilityTypeResponse>();
                }

                return new List<DisabilityTypeResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tipos de discapacidad activos");
                return new List<DisabilityTypeResponse>();
            }
        }
    }
}
