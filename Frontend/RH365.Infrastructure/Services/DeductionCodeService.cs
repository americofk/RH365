// ============================================================================
// Archivo: DeductionCodeService.cs (VERSIÓN CORREGIDA)
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/DeductionCodeService.cs
// Descripción:
//   - Servicio para comunicación con API de Códigos de Deducción
//   - Adaptado para API que usa skip/take y devuelve array directo
//   - Cumplimiento ISO 27001: Logging y manejo seguro de errores
// ============================================================================

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RH365.Core.Domain.Models.DeductionCode;

namespace RH365.Infrastructure.Services
{
    public interface IDeductionCodeService
    {
        Task<List<DeductionCodeResponse>> GetAllAsync(int skip = 0, int take = 100, CancellationToken ct = default);
        Task<DeductionCodeResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<DeductionCodeResponse> CreateAsync(CreateDeductionCodeRequest request, CancellationToken ct = default);
        Task<DeductionCodeResponse> UpdateAsync(long recId, UpdateDeductionCodeRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
        Task<List<DeductionCodeResponse>> GetEnabledAsync(CancellationToken ct = default);
        Task<List<DeductionCodeResponse>> GetDisabledAsync(CancellationToken ct = default);
    }

    public class DeductionCodeService : IDeductionCodeService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<DeductionCodeService> _logger;

        public DeductionCodeService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<DeductionCodeService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los códigos de deducción con paginación skip/take
        /// NOTA: El API devuelve un array directo, no un objeto con wrapper
        /// </summary>
        public async Task<List<DeductionCodeResponse>> GetAllAsync(
            int skip = 0,
            int take = 100,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("DeductionCodes")}?skip={skip}&take={take}";
                _logger.LogInformation($"Obteniendo códigos de deducción: skip={skip}, take={take}");

                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    
                    // El API devuelve array directo
                    var result = JsonSerializer.Deserialize<List<DeductionCodeResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    _logger.LogInformation($"Se obtuvieron {result?.Count ?? 0} códigos de deducción");
                    return result ?? new List<DeductionCodeResponse>();
                }

                _logger.LogWarning($"Error obteniendo códigos de deducción: {response.StatusCode}");
                return new List<DeductionCodeResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener códigos de deducción");
                throw;
            }
        }

        /// <summary>
        /// Obtener código de deducción por RecID
        /// </summary>
        public async Task<DeductionCodeResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("DeductionCodes")}/{recId}";
                _logger.LogInformation($"Obteniendo código de deducción con RecID: {recId}");

                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<DeductionCodeResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    _logger.LogInformation($"Código de deducción {recId} obtenido exitosamente");
                    return result;
                }

                _logger.LogWarning($"Código de deducción {recId} no encontrado: {response.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener código de deducción {recId}");
                throw;
            }
        }

        /// <summary>
        /// Crear nuevo código de deducción
        /// </summary>
        public async Task<DeductionCodeResponse> CreateAsync(CreateDeductionCodeRequest request, CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("DeductionCodes");
                _logger.LogInformation($"Creando código de deducción: {request.Name}");

                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<DeductionCodeResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    _logger.LogInformation($"Código de deducción '{request.Name}' creado exitosamente");
                    return result;
                }

                var errorContent = await response.Content.ReadAsStringAsync(ct);
                _logger.LogError($"Error al crear código de deducción: {response.StatusCode} - {errorContent}");
                throw new Exception($"Error al crear código de deducción: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear código de deducción");
                throw;
            }
        }

        /// <summary>
        /// Actualizar código de deducción existente
        /// </summary>
        public async Task<DeductionCodeResponse> UpdateAsync(
            long recId,
            UpdateDeductionCodeRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("DeductionCodes")}/{recId}";
                _logger.LogInformation($"Actualizando código de deducción con RecID: {recId}");

                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<DeductionCodeResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    _logger.LogInformation($"Código de deducción {recId} actualizado exitosamente");
                    return result;
                }

                var errorContent = await response.Content.ReadAsStringAsync(ct);
                _logger.LogError($"Error al actualizar código de deducción: {response.StatusCode} - {errorContent}");
                throw new Exception($"Error al actualizar código de deducción: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar código de deducción {recId}");
                throw;
            }
        }

        /// <summary>
        /// Eliminar código de deducción
        /// </summary>
        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("DeductionCodes")}/{recId}";
                _logger.LogInformation($"Eliminando código de deducción con RecID: {recId}");

                var response = await _httpClient.DeleteAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Código de deducción {recId} eliminado exitosamente");
                    return true;
                }

                _logger.LogWarning($"Error al eliminar código de deducción {recId}: {response.StatusCode}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar código de deducción {recId}");
                return false;
            }
        }

        /// <summary>
        /// Obtener códigos de deducción activos
        /// </summary>
        public async Task<List<DeductionCodeResponse>> GetEnabledAsync(CancellationToken ct = default)
        {
            try
            {
                var url = _urlsService.GetUrl("DeductionCodes.Enabled");
                _logger.LogInformation("Obteniendo códigos de deducción activos");

                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<List<DeductionCodeResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<DeductionCodeResponse>();

                    _logger.LogInformation($"Se obtuvieron {result.Count} códigos de deducción activos");
                    return result;
                }

                _logger.LogWarning($"Error obteniendo códigos de deducción activos: {response.StatusCode}");
                return new List<DeductionCodeResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener códigos de deducción activos");
                return new List<DeductionCodeResponse>();
            }
        }

        /// <summary>
        /// Obtener códigos de deducción inactivos
        /// </summary>
        public async Task<List<DeductionCodeResponse>> GetDisabledAsync(CancellationToken ct = default)
        {
            try
            {
                var url = _urlsService.GetUrl("DeductionCodes.Disabled");
                _logger.LogInformation("Obteniendo códigos de deducción inactivos");

                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<List<DeductionCodeResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<DeductionCodeResponse>();

                    _logger.LogInformation($"Se obtuvieron {result.Count} códigos de deducción inactivos");
                    return result;
                }

                _logger.LogWarning($"Error obteniendo códigos de deducción inactivos: {response.StatusCode}");
                return new List<DeductionCodeResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener códigos de deducción inactivos");
                return new List<DeductionCodeResponse>();
            }
        }
    }
}
