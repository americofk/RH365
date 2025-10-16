// ============================================================================
// Archivo: ProjectService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/ProjectService.cs
// Descripción:
//   - Servicio para comunicación con API de Proyectos
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
using RH365.Core.Domain.Models.Project;

namespace RH365.Infrastructure.Services
{
    public interface IProjectService
    {
        Task<ProjectListResponse> GetAllAsync(int pageNumber = 1, int pageSize = 100, CancellationToken ct = default);
        Task<ProjectResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<ProjectResponse> CreateAsync(CreateProjectRequest request, CancellationToken ct = default);
        Task<ProjectResponse> UpdateAsync(long recId, UpdateProjectRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
        Task<List<ProjectResponse>> GetEnabledAsync(CancellationToken ct = default);
    }

    public class ProjectService : IProjectService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<ProjectService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los proyectos paginados
        /// </summary>
        public async Task<ProjectListResponse> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 100,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Projects")}?pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<ProjectListResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new ProjectListResponse();
                }

                _logger.LogWarning($"Error obteniendo proyectos: {response.StatusCode}");
                return new ProjectListResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener proyectos");
                throw;
            }
        }

        /// <summary>
        /// Obtener proyecto por RecID
        /// </summary>
        public async Task<ProjectResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Projects")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<ProjectResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener proyecto {recId}");
                throw;
            }
        }

        /// <summary>
        /// Crear nuevo proyecto
        /// </summary>
        public async Task<ProjectResponse> CreateAsync(CreateProjectRequest request, CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("Projects");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<ProjectResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al crear proyecto: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear proyecto");
                throw;
            }
        }

        /// <summary>
        /// Actualizar proyecto existente
        /// </summary>
        public async Task<ProjectResponse> UpdateAsync(
            long recId,
            UpdateProjectRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("Projects")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<ProjectResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al actualizar proyecto: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar proyecto {recId}");
                throw;
            }
        }

        /// <summary>
        /// Eliminar proyecto
        /// </summary>
        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Projects")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar proyecto {recId}");
                return false;
            }
        }

        /// <summary>
        /// Obtener proyectos activos
        /// </summary>
        public async Task<List<ProjectResponse>> GetEnabledAsync(CancellationToken ct = default)
        {
            try
            {
                var url = _urlsService.GetUrl("Projects.Enabled");
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<List<ProjectResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<ProjectResponse>();
                }

                return new List<ProjectResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener proyectos activos");
                return new List<ProjectResponse>();
            }
        }
    }
}