// ============================================================================
// Archivo: EarningCodeService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/EarningCodeService.cs
// Descripción:
//   - Servicio para comunicación con API de Códigos de Nómina
//   - Maneja CRUD completo y exportación de datos
//   - Usa modelos de RH365.Core
// Estándar: ISO 27001 - Gestión segura de información de nómina
// ============================================================================

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RH365.Core.Domain.Models.EarningCode;

namespace RH365.Infrastructure.Services
{
    public interface IEarningCodeService
    {
        Task<EarningCodeListResponse> GetAllAsync(int pageNumber = 1, int pageSize = 100, CancellationToken ct = default);
        Task<EarningCodeResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<EarningCodeResponse> CreateAsync(CreateEarningCodeRequest request, CancellationToken ct = default);
        Task<EarningCodeResponse> UpdateAsync(long recId, UpdateEarningCodeRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
        Task<List<EarningCodeResponse>> GetEnabledAsync(CancellationToken ct = default);
    }

    public class EarningCodeService : IEarningCodeService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<EarningCodeService> _logger;

        public EarningCodeService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<EarningCodeService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los códigos de nómina paginados
        /// </summary>
        public async Task<EarningCodeListResponse> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 100,
            CancellationToken ct = default)
        {
            try
            {
                var skip = (pageNumber - 1) * pageSize;
                var url = $"{_urlsService.GetUrl("EarningCodes")}?skip={skip}&take={pageSize}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    
                    // Intentar deserializar como array directo
                    var directArray = JsonSerializer.Deserialize<List<EarningCodeResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    
                    if (directArray != null)
                    {
                        return new EarningCodeListResponse
                        {
                            Data = directArray,
                            TotalCount = directArray.Count,
                            PageNumber = pageNumber,
                            PageSize = pageSize,
                            TotalPages = 1,
                            HasNextPage = false,
                            HasPreviousPage = false
                        };
                    }
                    
                    // Si no es array directo, intentar como objeto con Data
                    var result = JsonSerializer.Deserialize<EarningCodeListResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new EarningCodeListResponse();
                }

                _logger.LogWarning($"Error obteniendo códigos de nómina: {response.StatusCode}");
                return new EarningCodeListResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener códigos de nómina");
                throw;
            }
        }

        /// <summary>
        /// Obtener código de nómina por RecID
        /// </summary>
        public async Task<EarningCodeResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("EarningCodes")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<EarningCodeResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener código de nómina {recId}");
                throw;
            }
        }

        /// <summary>
        /// Crear nuevo código de nómina
        /// </summary>
        public async Task<EarningCodeResponse> CreateAsync(CreateEarningCodeRequest request, CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("EarningCodes");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<EarningCodeResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al crear código de nómina: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear código de nómina");
                throw;
            }
        }

        /// <summary>
        /// Actualizar código de nómina existente
        /// </summary>
        public async Task<EarningCodeResponse> UpdateAsync(
            long recId,
            UpdateEarningCodeRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("EarningCodes")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<EarningCodeResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al actualizar código de nómina: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar código de nómina {recId}");
                throw;
            }
        }

        /// <summary>
        /// Eliminar código de nómina
        /// </summary>
        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("EarningCodes")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar código de nómina {recId}");
                return false;
            }
        }

        /// <summary>
        /// Obtener códigos de nómina activos
        /// </summary>
        public async Task<List<EarningCodeResponse>> GetEnabledAsync(CancellationToken ct = default)
        {
            try
            {
                var url = _urlsService.GetUrl("EarningCodes.Enabled");
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<List<EarningCodeResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<EarningCodeResponse>();
                }

                return new List<EarningCodeResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener códigos de nómina activos");
                return new List<EarningCodeResponse>();
            }
        }
    }
}
