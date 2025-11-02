// ============================================================================
// Archivo: PositionService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/PositionService.cs
// Descripción:
//   - Servicio para comunicación con API de Posiciones
//   - Maneja CRUD completo y consultas especializadas
//   - Usa modelos de RH365.Core
// Estándar: ISO 27001 - Control A.12.1.1 (Procedimientos operacionales)
//           Control A.12.4.1 (Registro de eventos - logging completo)
// ============================================================================

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RH365.Core.Domain.Models.Position;

namespace RH365.Infrastructure.Services
{
    public interface IPositionService
    {
        Task<PositionListResponse> GetAllAsync(int pageNumber = 1, int pageSize = 100, CancellationToken ct = default);
        Task<PositionResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<PositionResponse> CreateAsync(CreatePositionRequest request, CancellationToken ct = default);
        Task<PositionResponse> UpdateAsync(long recId, UpdatePositionRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
        Task<List<PositionResponse>> GetEnabledAsync(CancellationToken ct = default);
        Task<List<PositionResponse>> GetVacantAsync(CancellationToken ct = default);
    }

    public class PositionService : IPositionService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<PositionService> _logger;

        public PositionService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<PositionService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todas las posiciones paginadas
        /// </summary>
        public async Task<PositionListResponse> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 100,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Positions")}?pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<PositionListResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new PositionListResponse();
                }

                _logger.LogWarning($"Error obteniendo posiciones: {response.StatusCode}");
                return new PositionListResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener posiciones");
                throw;
            }
        }

        /// <summary>
        /// Obtener posición por RecID
        /// </summary>
        public async Task<PositionResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Positions")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<PositionResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener posición {recId}");
                throw;
            }
        }

        /// <summary>
        /// Crear nueva posición
        /// </summary>
        public async Task<PositionResponse> CreateAsync(CreatePositionRequest request, CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("Positions");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<PositionResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al crear posición: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear posición");
                throw;
            }
        }

        /// <summary>
        /// Actualizar posición existente
        /// </summary>
        public async Task<PositionResponse> UpdateAsync(
            long recId,
            UpdatePositionRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("Positions")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<PositionResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al actualizar posición: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar posición {recId}");
                throw;
            }
        }

        /// <summary>
        /// Eliminar posición
        /// </summary>
        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Positions")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar posición {recId}");
                return false;
            }
        }

        /// <summary>
        /// Obtener posiciones activas
        /// </summary>
        public async Task<List<PositionResponse>> GetEnabledAsync(CancellationToken ct = default)
        {
            try
            {
                var url = _urlsService.GetUrl("Positions.Enabled");
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<List<PositionResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<PositionResponse>();
                }

                return new List<PositionResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener posiciones activas");
                return new List<PositionResponse>();
            }
        }

        /// <summary>
        /// Obtener posiciones vacantes
        /// </summary>
        public async Task<List<PositionResponse>> GetVacantAsync(CancellationToken ct = default)
        {
            try
            {
                var url = _urlsService.GetUrl("Positions.Vacant");
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<List<PositionResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<PositionResponse>();
                }

                return new List<PositionResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener posiciones vacantes");
                return new List<PositionResponse>();
            }
        }
    }
}
