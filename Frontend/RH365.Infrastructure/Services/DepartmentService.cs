// ============================================================================
// Archivo: DepartmentService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/DepartmentService.cs
// Descripción:
//   - Servicio para comunicación con API de Departamentos
//   - Maneja CRUD completo y exportación de datos
//   - Usa modelos de RH365.Core
// Estándar: ISO 27001 - Comunicación segura con API
// ============================================================================

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RH365.Core.Domain.Models.Department;

namespace RH365.Infrastructure.Services
{
    /// <summary>
    /// Interfaz para el servicio de departamentos
    /// </summary>
    public interface IDepartmentService
    {
        Task<DepartmentListResponse> GetAllAsync(int pageNumber = 1, int pageSize = 100, CancellationToken ct = default);
        Task<DepartmentResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<DepartmentResponse> CreateAsync(CreateDepartmentRequest request, CancellationToken ct = default);
        Task<DepartmentResponse> UpdateAsync(long recId, UpdateDepartmentRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
        Task<List<DepartmentResponse>> GetEnabledAsync(CancellationToken ct = default);
    }

    /// <summary>
    /// Servicio para gestión de departamentos mediante API REST
    /// </summary>
    public class DepartmentService : IDepartmentService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<DepartmentService> _logger;

        public DepartmentService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<DepartmentService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los departamentos paginados
        /// </summary>
        public async Task<DepartmentListResponse> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 100,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Departments")}?pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<DepartmentListResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new DepartmentListResponse();
                }

                _logger.LogWarning($"Error obteniendo departamentos: {response.StatusCode}");
                return new DepartmentListResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener departamentos");
                throw;
            }
        }

        /// <summary>
        /// Obtener departamento por RecID
        /// </summary>
        public async Task<DepartmentResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Departments")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<DepartmentResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener departamento {recId}");
                throw;
            }
        }

        /// <summary>
        /// Crear nuevo departamento
        /// </summary>
        public async Task<DepartmentResponse> CreateAsync(CreateDepartmentRequest request, CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("Departments");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<DepartmentResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al crear departamento: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear departamento");
                throw;
            }
        }

        /// <summary>
        /// Actualizar departamento existente
        /// </summary>
        public async Task<DepartmentResponse> UpdateAsync(
            long recId,
            UpdateDepartmentRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("Departments")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<DepartmentResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al actualizar departamento: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar departamento {recId}");
                throw;
            }
        }

        /// <summary>
        /// Eliminar departamento
        /// </summary>
        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Departments")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar departamento {recId}");
                return false;
            }
        }

        /// <summary>
        /// Obtener departamentos activos
        /// </summary>
        public async Task<List<DepartmentResponse>> GetEnabledAsync(CancellationToken ct = default)
        {
            try
            {
                var url = _urlsService.GetUrl("Departments.Enabled");
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<List<DepartmentResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<DepartmentResponse>();
                }

                return new List<DepartmentResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener departamentos activos");
                return new List<DepartmentResponse>();
            }
        }
    }
}
