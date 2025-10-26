// ============================================================================
// Archivo: ProjectCategoryService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/ProjectCategoryService.cs
// Descripción: Servicio para comunicación con API de Categorías de Proyecto
// ============================================================================

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RH365.Core.Domain.Models.ProjectCategory;

namespace RH365.Infrastructure.Services
{
    public interface IProjectCategoryService
    {
        Task<ProjectCategoryListResponse> GetAllAsync(int pageNumber = 1, int pageSize = 100, CancellationToken ct = default);
        Task<ProjectCategoryResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<ProjectCategoryResponse> CreateAsync(CreateProjectCategoryRequest request, CancellationToken ct = default);
        Task<ProjectCategoryResponse> UpdateAsync(long recId, UpdateProjectCategoryRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
        Task<List<ProjectCategoryResponse>> GetEnabledAsync(CancellationToken ct = default);
    }

    public class ProjectCategoryService : IProjectCategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<ProjectCategoryService> _logger;

        public ProjectCategoryService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<ProjectCategoryService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        public async Task<ProjectCategoryListResponse> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 100,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("ProjectCategories")}?pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<ProjectCategoryListResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new ProjectCategoryListResponse();
                }

                _logger.LogWarning($"Error obteniendo categorías: {response.StatusCode}");
                return new ProjectCategoryListResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener categorías");
                throw;
            }
        }

        public async Task<ProjectCategoryResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("ProjectCategories")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<ProjectCategoryResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener categoría {recId}");
                throw;
            }
        }

        public async Task<ProjectCategoryResponse> CreateAsync(CreateProjectCategoryRequest request, CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("ProjectCategories");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<ProjectCategoryResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al crear categoría: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear categoría");
                throw;
            }
        }

        public async Task<ProjectCategoryResponse> UpdateAsync(
            long recId,
            UpdateProjectCategoryRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("ProjectCategories")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<ProjectCategoryResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al actualizar categoría: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar categoría {recId}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("ProjectCategories")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar categoría {recId}");
                return false;
            }
        }

        public async Task<List<ProjectCategoryResponse>> GetEnabledAsync(CancellationToken ct = default)
        {
            try
            {
                var url = _urlsService.GetUrl("ProjectCategories.Enabled");
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<List<ProjectCategoryResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<ProjectCategoryResponse>();
                }

                return new List<ProjectCategoryResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener categorías activas");
                return new List<ProjectCategoryResponse>();
            }
        }
    }
}
