// ============================================================================
// Archivo: CourseLocationService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/CourseLocationService.cs
// Descripción:
//   - Servicio para comunicación con API de Ubicaciones de Cursos
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
using RH365.Core.Domain.Models.CourseLocation;

namespace RH365.Infrastructure.Services
{
    public interface ICourseLocationService
    {
        Task<CourseLocationListResponse> GetAllAsync(int pageNumber = 1, int pageSize = 100, CancellationToken ct = default);
        Task<CourseLocationResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<CourseLocationResponse> CreateAsync(CreateCourseLocationRequest request, CancellationToken ct = default);
        Task<CourseLocationResponse> UpdateAsync(long recId, UpdateCourseLocationRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
    }

    public class CourseLocationService : ICourseLocationService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<CourseLocationService> _logger;

        public CourseLocationService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<CourseLocationService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todas las ubicaciones de cursos paginadas
        /// </summary>
        public async Task<CourseLocationListResponse> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 100,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("CourseLocations")}?pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<CourseLocationListResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new CourseLocationListResponse();
                }

                _logger.LogWarning($"Error obteniendo ubicaciones de cursos: {response.StatusCode}");
                return new CourseLocationListResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ubicaciones de cursos");
                throw;
            }
        }

        /// <summary>
        /// Obtener ubicación de curso por RecID
        /// </summary>
        public async Task<CourseLocationResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("CourseLocations")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<CourseLocationResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener ubicación de curso {recId}");
                throw;
            }
        }

        /// <summary>
        /// Crear nueva ubicación de curso
        /// </summary>
        public async Task<CourseLocationResponse> CreateAsync(CreateCourseLocationRequest request, CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("CourseLocations");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<CourseLocationResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al crear ubicación de curso: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear ubicación de curso");
                throw;
            }
        }

        /// <summary>
        /// Actualizar ubicación de curso existente
        /// </summary>
        public async Task<CourseLocationResponse> UpdateAsync(
            long recId,
            UpdateCourseLocationRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("CourseLocations")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<CourseLocationResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al actualizar ubicación de curso: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar ubicación de curso {recId}");
                throw;
            }
        }

        /// <summary>
        /// Eliminar ubicación de curso
        /// </summary>
        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("CourseLocations")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar ubicación de curso {recId}");
                return false;
            }
        }
    }
}
