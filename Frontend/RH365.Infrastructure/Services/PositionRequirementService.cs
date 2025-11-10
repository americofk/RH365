// ============================================================================
// Archivo: PositionRequirementService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/PositionRequirementService.cs
// Descripción:
//   - Servicio para comunicación con API de Requisitos de Posición
//   - Maneja CRUD completo y consultas por posición
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
using RH365.Core.Domain.Models.PositionRequirement;

namespace RH365.Infrastructure.Services
{
    public interface IPositionRequirementService
    {
        Task<PositionRequirementListResponse> GetAllAsync(int pageNumber = 1, int pageSize = 100, CancellationToken ct = default);
        Task<PositionRequirementResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<List<PositionRequirementResponse>> GetByPositionAsync(long positionRefRecID, CancellationToken ct = default);
        Task<PositionRequirementResponse> CreateAsync(CreatePositionRequirementRequest request, CancellationToken ct = default);
        Task<PositionRequirementResponse> UpdateAsync(long recId, UpdatePositionRequirementRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
    }

    public class PositionRequirementService : IPositionRequirementService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<PositionRequirementService> _logger;

        public PositionRequirementService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<PositionRequirementService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los requisitos paginados
        /// </summary>
        public async Task<PositionRequirementListResponse> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 100,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("PositionRequirements")}?pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<PositionRequirementListResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new PositionRequirementListResponse();
                }

                _logger.LogWarning($"Error obteniendo requisitos: {response.StatusCode}");
                return new PositionRequirementListResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener requisitos de posición");
                throw;
            }
        }

        /// <summary>
        /// Obtener requisito por RecID
        /// </summary>
        public async Task<PositionRequirementResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("PositionRequirements")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<PositionRequirementResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener requisito {recId}");
                throw;
            }
        }

        /// <summary>
        /// Obtener requisitos de una posición específica
        /// </summary>
        public async Task<List<PositionRequirementResponse>> GetByPositionAsync(
            long positionRefRecID,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("PositionRequirements")}/position/{positionRefRecID}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<List<PositionRequirementResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<PositionRequirementResponse>();
                }

                _logger.LogWarning($"No se encontraron requisitos para la posición {positionRefRecID}");
                return new List<PositionRequirementResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener requisitos de la posición {positionRefRecID}");
                throw;
            }
        }

        /// <summary>
        /// Crear nuevo requisito
        /// </summary>
        public async Task<PositionRequirementResponse> CreateAsync(
            CreatePositionRequirementRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("PositionRequirements");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<PositionRequirementResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                var errorContent = await response.Content.ReadAsStringAsync(ct);
                throw new Exception($"Error al crear requisito: {response.StatusCode} - {errorContent}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear requisito de posición");
                throw;
            }
        }

        /// <summary>
        /// Actualizar requisito existente
        /// </summary>
        public async Task<PositionRequirementResponse> UpdateAsync(
            long recId,
            UpdatePositionRequirementRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("PositionRequirements")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<PositionRequirementResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                var errorContent = await response.Content.ReadAsStringAsync(ct);
                throw new Exception($"Error al actualizar requisito: {response.StatusCode} - {errorContent}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar requisito {recId}");
                throw;
            }
        }

        /// <summary>
        /// Eliminar requisito
        /// </summary>
        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("PositionRequirements")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Requisito {recId} eliminado exitosamente");
                    return true;
                }

                _logger.LogWarning($"Error al eliminar requisito {recId}: {response.StatusCode}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar requisito {recId}");
                return false;
            }
        }
    }
}