// ============================================================================
// Archivo: CourseInstructorService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/CourseInstructorService.cs
// Descripción:
//   - Servicio para comunicación con API de Instructores de Curso
//   - Maneja CRUD completo y filtrado por curso
//   - Usa modelos de RH365.Core
// Estándar: ISO 27001 - Gestión segura de servicios de datos
// ============================================================================

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RH365.Core.Domain.Models.CourseInstructor;

namespace RH365.Infrastructure.Services
{
    public interface ICourseInstructorService
    {
        Task<CourseInstructorListResponse> GetAllAsync(int pageNumber = 1, int pageSize = 100, CancellationToken ct = default);
        Task<CourseInstructorResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<List<CourseInstructorResponse>> GetByCourseAsync(long courseRecId, CancellationToken ct = default);
        Task<CourseInstructorResponse> CreateAsync(CreateCourseInstructorRequest request, CancellationToken ct = default);
        Task<CourseInstructorResponse> UpdateAsync(long recId, UpdateCourseInstructorRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
    }

    public class CourseInstructorService : ICourseInstructorService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<CourseInstructorService> _logger;

        public CourseInstructorService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<CourseInstructorService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los instructores de curso paginados
        /// </summary>
        public async Task<CourseInstructorListResponse> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 100,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("CourseInstructors")}?pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<CourseInstructorListResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new CourseInstructorListResponse();
                }

                _logger.LogWarning($"Error obteniendo instructores de curso: {response.StatusCode}");
                return new CourseInstructorListResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener instructores de curso");
                throw;
            }
        }

        /// <summary>
        /// Obtener instructor de curso por RecID
        /// </summary>
        public async Task<CourseInstructorResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("CourseInstructors")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<CourseInstructorResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener instructor de curso {recId}");
                throw;
            }
        }

        /// <summary>
        /// Obtener instructores por curso específico
        /// </summary>
        public async Task<List<CourseInstructorResponse>> GetByCourseAsync(long courseRecId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("CourseInstructors")}/ByCourse/{courseRecId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<List<CourseInstructorResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<CourseInstructorResponse>();
                }

                _logger.LogWarning($"No se encontraron instructores para el curso {courseRecId}");
                return new List<CourseInstructorResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener instructores del curso {courseRecId}");
                return new List<CourseInstructorResponse>();
            }
        }

        /// <summary>
        /// Crear nuevo instructor de curso
        /// </summary>
        public async Task<CourseInstructorResponse> CreateAsync(CreateCourseInstructorRequest request, CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("CourseInstructors");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<CourseInstructorResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al crear instructor de curso: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear instructor de curso");
                throw;
            }
        }

        /// <summary>
        /// Actualizar instructor de curso existente
        /// </summary>
        public async Task<CourseInstructorResponse> UpdateAsync(
            long recId,
            UpdateCourseInstructorRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("CourseInstructors")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<CourseInstructorResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al actualizar instructor de curso: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar instructor de curso {recId}");
                throw;
            }
        }

        /// <summary>
        /// Eliminar instructor de curso
        /// </summary>
        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("CourseInstructors")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar instructor de curso {recId}");
                return false;
            }
        }
    }
}
