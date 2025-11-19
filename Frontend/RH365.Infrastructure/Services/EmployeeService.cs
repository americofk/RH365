// ============================================================================
// Archivo: EmployeeService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/EmployeeService.cs
// Descripción:
//   - Servicio para comunicación con API de Empleados
//   - Maneja CRUD completo y operaciones especiales
//   - Usa modelos de RH365.Core
// Estándar: ISO 27001 - Registro de operaciones y trazabilidad
// ============================================================================

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RH365.Core.Domain.Models.Employee;

namespace RH365.Infrastructure.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeListResponse> GetAllAsync(int pageNumber = 1, int pageSize = 100, CancellationToken ct = default);
        Task<EmployeeResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request, CancellationToken ct = default);
        Task<EmployeeResponse> UpdateAsync(long recId, UpdateEmployeeRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
        Task<List<EmployeeResponse>> GetActiveAsync(CancellationToken ct = default);
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<EmployeeService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los empleados paginados
        /// </summary>
        public async Task<EmployeeListResponse> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 100,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Employees")}?pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<EmployeeListResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new EmployeeListResponse();
                }

                _logger.LogWarning($"Error obteniendo empleados: {response.StatusCode}");
                return new EmployeeListResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empleados");
                throw;
            }
        }

        /// <summary>
        /// Obtener empleado por RecID
        /// </summary>
        public async Task<EmployeeResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Employees")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<EmployeeResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener empleado {recId}");
                throw;
            }
        }

        /// <summary>
        /// Crear nuevo empleado
        /// </summary>
        public async Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request, CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("Employees");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<EmployeeResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al crear empleado: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear empleado");
                throw;
            }
        }

        /// <summary>
        /// Actualizar empleado existente
        /// </summary>
        public async Task<EmployeeResponse> UpdateAsync(
            long recId,
            UpdateEmployeeRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("Employees")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<EmployeeResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al actualizar empleado: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar empleado {recId}");
                throw;
            }
        }

        /// <summary>
        /// Eliminar empleado
        /// </summary>
        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Employees")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar empleado {recId}");
                return false;
            }
        }

        /// <summary>
        /// Obtener empleados activos
        /// </summary>
        public async Task<List<EmployeeResponse>> GetActiveAsync(CancellationToken ct = default)
        {
            try
            {
                var url = _urlsService.GetUrl("Employees.Active");
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<List<EmployeeResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<EmployeeResponse>();
                }

                return new List<EmployeeResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empleados activos");
                return new List<EmployeeResponse>();
            }
        }
    }
}