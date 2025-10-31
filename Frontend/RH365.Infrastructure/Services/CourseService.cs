// ============================================================================
// Archivo: CourseService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/CourseService.cs
// Descripción:
//   - Servicio para comunicación con API de Cursos
//   - Maneja CRUD completo y exportación de datos
//   - Usa modelos de RH365.Core
// Estándar: ISO 27001 - Seguridad en comunicación con APIs
// ============================================================================

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RH365.Core.Domain.Models.Course;

namespace RH365.Infrastructure.Services
{
    public interface ICourseService
    {
        Task<CourseListResponse> GetAllAsync(int pageNumber = 1, int pageSize = 100, CancellationToken ct = default);
        Task<CourseResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<CourseResponse> CreateAsync(CreateCourseRequest request, CancellationToken ct = default);
        Task<CourseResponse> UpdateAsync(long recId, UpdateCourseRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
        Task<List<CourseResponse>> GetEnabledAsync(CancellationToken ct = default);
    }

    public class CourseService : ICourseService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<CourseService> _logger;

        public CourseService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<CourseService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los cursos paginados
        /// </summary>
        public async Task<CourseListResponse> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 100,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Courses")}?pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<CourseListResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new CourseListResponse();
                }

                _logger.LogWarning($"Error obteniendo cursos: {response.StatusCode}");
                return new CourseListResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cursos");
                throw;
            }
        }

        /// <summary>
        /// Obtener curso por RecID
        /// </summary>
        public async Task<CourseResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Courses")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<CourseResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener curso {recId}");
                throw;
            }
        }

        /// <summary>
        /// Crear nuevo curso
        /// </summary>
        public async Task<CourseResponse> CreateAsync(CreateCourseRequest request, CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("Courses");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<CourseResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al crear curso: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear curso");
                throw;
            }
        }

        /// <summary>
        /// Actualizar curso existente
        /// </summary>
        public async Task<CourseResponse> UpdateAsync(
            long recId,
            UpdateCourseRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("Courses")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<CourseResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al actualizar curso: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar curso {recId}");
                throw;
            }
        }

        /// <summary>
        /// Eliminar curso
        /// </summary>
        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Courses")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar curso {recId}");
                return false;
            }
        }

        /// <summary>
        /// Obtener cursos activos
        /// </summary>
        public async Task<List<CourseResponse>> GetEnabledAsync(CancellationToken ct = default)
        {
            try
            {
                var url = _urlsService.GetUrl("Courses.Enabled");
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<List<CourseResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<CourseResponse>();
                }

                return new List<CourseResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cursos activos");
                return new List<CourseResponse>();
            }
        }
    }
}
