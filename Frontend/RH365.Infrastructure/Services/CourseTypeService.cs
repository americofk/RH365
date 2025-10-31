// ============================================================================
// Archivo: CourseTypeService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/CourseTypeService.cs
// Descripción:
//   - Servicio para comunicación con API de Tipos de Curso
//   - Maneja CRUD completo y exportación de datos
//   - Usa modelos de RH365.Core
// Estándar: ISO 27001 - Gestión segura de comunicación con API
// ============================================================================

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RH365.Core.Domain.Models.CourseType;

namespace RH365.Infrastructure.Services
{
    public interface ICourseTypeService
    {
        Task<CourseTypeListResponse> GetAllAsync(int pageNumber = 1, int pageSize = 100, CancellationToken ct = default);
        Task<CourseTypeResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<CourseTypeResponse> CreateAsync(CreateCourseTypeRequest request, CancellationToken ct = default);
        Task<CourseTypeResponse> UpdateAsync(long recId, UpdateCourseTypeRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
        Task<List<CourseTypeResponse>> GetEnabledAsync(CancellationToken ct = default);
    }

    public class CourseTypeService : ICourseTypeService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<CourseTypeService> _logger;

        public CourseTypeService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<CourseTypeService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los tipos de curso paginados
        /// </summary>
        public async Task<CourseTypeListResponse> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 100,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("CourseTypes")}?pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<CourseTypeListResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new CourseTypeListResponse();
                }

                _logger.LogWarning($"Error obteniendo tipos de curso: {response.StatusCode}");
                return new CourseTypeListResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tipos de curso");
                throw;
            }
        }

        /// <summary>
        /// Obtener tipo de curso por RecID
        /// </summary>
        public async Task<CourseTypeResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("CourseTypes")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<CourseTypeResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener tipo de curso {recId}");
                throw;
            }
        }

        /// <summary>
        /// Crear nuevo tipo de curso
        /// </summary>
        public async Task<CourseTypeResponse> CreateAsync(CreateCourseTypeRequest request, CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("CourseTypes");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<CourseTypeResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al crear tipo de curso: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear tipo de curso");
                throw;
            }
        }

        /// <summary>
        /// Actualizar tipo de curso existente
        /// </summary>
        public async Task<CourseTypeResponse> UpdateAsync(
            long recId,
            UpdateCourseTypeRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("CourseTypes")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<CourseTypeResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al actualizar tipo de curso: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar tipo de curso {recId}");
                throw;
            }
        }

        /// <summary>
        /// Eliminar tipo de curso
        /// </summary>
        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("CourseTypes")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar tipo de curso {recId}");
                return false;
            }
        }

        /// <summary>
        /// Obtener tipos de curso activos
        /// </summary>
        public async Task<List<CourseTypeResponse>> GetEnabledAsync(CancellationToken ct = default)
        {
            try
            {
                var url = _urlsService.GetUrl("CourseTypes.Enabled");
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<List<CourseTypeResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<CourseTypeResponse>();
                }

                return new List<CourseTypeResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tipos de curso activos");
                return new List<CourseTypeResponse>();
            }
        }
    }
}
