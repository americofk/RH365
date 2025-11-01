// ============================================================================
// Archivo: JobService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/JobService.cs
// Descripción:
//   - Servicio para comunicación con API de Cargos
//   - Maneja CRUD completo
//   - Usa modelos de RH365.Core
// Estándar: ISO 27001 - Seguridad en comunicaciones API
// ============================================================================

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RH365.Core.Domain.Models.Job;

namespace RH365.Infrastructure.Services
{
    public interface IJobService
    {
        Task<JobListResponse> GetAllAsync(int pageNumber = 1, int pageSize = 100, CancellationToken ct = default);
        Task<JobResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<JobResponse> CreateAsync(CreateJobRequest request, CancellationToken ct = default);
        Task<JobResponse> UpdateAsync(long recId, UpdateJobRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
        Task<List<JobResponse>> GetEnabledAsync(CancellationToken ct = default);
    }

    public class JobService : IJobService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<JobService> _logger;

        public JobService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<JobService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los cargos paginados
        /// </summary>
        public async Task<JobListResponse> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 100,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Jobs")}?pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<JobListResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new JobListResponse();
                }

                _logger.LogWarning($"Error obteniendo cargos: {response.StatusCode}");
                return new JobListResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cargos");
                throw;
            }
        }

        /// <summary>
        /// Obtener cargo por RecID
        /// </summary>
        public async Task<JobResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Jobs")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<JobResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener cargo {recId}");
                throw;
            }
        }

        /// <summary>
        /// Crear nuevo cargo
        /// </summary>
        public async Task<JobResponse> CreateAsync(CreateJobRequest request, CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("Jobs");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<JobResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al crear cargo: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear cargo");
                throw;
            }
        }

        /// <summary>
        /// Actualizar cargo existente
        /// </summary>
        public async Task<JobResponse> UpdateAsync(
            long recId,
            UpdateJobRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("Jobs")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<JobResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al actualizar cargo: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar cargo {recId}");
                throw;
            }
        }

        /// <summary>
        /// Eliminar cargo
        /// </summary>
        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Jobs")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar cargo {recId}");
                return false;
            }
        }

        /// <summary>
        /// Obtener cargos activos
        /// </summary>
        public async Task<List<JobResponse>> GetEnabledAsync(CancellationToken ct = default)
        {
            try
            {
                var url = _urlsService.GetUrl("Jobs.Enabled");
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<List<JobResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<JobResponse>();
                }

                return new List<JobResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cargos activos");
                return new List<JobResponse>();
            }
        }
    }
}
