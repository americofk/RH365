// ============================================================================
// Archivo: EmployeeAddressService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/EmployeeAddressService.cs
// Descripcion:
//   - Servicio para comunicacion con API de Direcciones de Empleados
//   - Endpoint correcto: /api/EmployeesAddress (con 's')
//   - Maneja CRUD completo
//   - ISO 27001: Trazabilidad de operaciones sobre direcciones
// ============================================================================

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RH365.Core.Domain.Models.EmployeeAddress;

namespace RH365.Infrastructure.Services
{
    public interface IEmployeeAddressService
    {
        Task<EmployeeAddressListResponse> GetAllAsync(int pageNumber = 1, int pageSize = 100, CancellationToken ct = default);
        Task<EmployeeAddressResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<EmployeeAddressResponse> CreateAsync(CreateEmployeeAddressRequest request, CancellationToken ct = default);
        Task<EmployeeAddressResponse> UpdateAsync(long recId, UpdateEmployeeAddressRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
    }

    public class EmployeeAddressService : IEmployeeAddressService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<EmployeeAddressService> _logger;

        public EmployeeAddressService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<EmployeeAddressService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        public async Task<EmployeeAddressListResponse> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 100,
            CancellationToken ct = default)
        {
            try
            {
                // Endpoint correcto con 's' en Employees
                var url = $"{_urlsService.GetUrl("EmployeesAddress")}?pageNumber={pageNumber}&pageSize={pageSize}";
                _logger.LogInformation($"Consultando direcciones: Pagina {pageNumber}, Tamaño {pageSize}");

                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<EmployeeAddressListResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    _logger.LogInformation($"Consulta exitosa: {result?.TotalCount ?? 0} direcciones encontradas");
                    return result ?? new EmployeeAddressListResponse();
                }

                _logger.LogWarning($"Error obteniendo direcciones: {response.StatusCode}");
                return new EmployeeAddressListResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener direcciones de empleados");
                throw;
            }
        }

        public async Task<EmployeeAddressResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("EmployeesAddress")}/{recId}";
                _logger.LogInformation($"Consultando direccion con RecID: {recId}");

                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<EmployeeAddressResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    _logger.LogInformation($"Direccion {recId} obtenida exitosamente");
                    return result;
                }

                _logger.LogWarning($"Direccion {recId} no encontrada: {response.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener direccion {recId}");
                throw;
            }
        }

        public async Task<EmployeeAddressResponse> CreateAsync(
            CreateEmployeeAddressRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("EmployeesAddress");
                _logger.LogInformation($"Creando direccion para empleado: {request.EmployeeRefRecID}");

                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<EmployeeAddressResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    _logger.LogInformation($"Direccion creada exitosamente con RecID: {result?.RecID}");
                    return result;
                }

                var errorContent = await response.Content.ReadAsStringAsync(ct);
                _logger.LogError($"Error al crear direccion: {response.StatusCode} - {errorContent}");
                throw new Exception($"Error al crear direccion: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear direccion de empleado");
                throw;
            }
        }

        public async Task<EmployeeAddressResponse> UpdateAsync(
            long recId,
            UpdateEmployeeAddressRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("EmployeesAddress")}/{recId}";
                _logger.LogInformation($"Actualizando direccion: {recId}");

                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<EmployeeAddressResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    _logger.LogInformation($"Direccion {recId} actualizada exitosamente");
                    return result;
                }

                var errorContent = await response.Content.ReadAsStringAsync(ct);
                _logger.LogError($"Error al actualizar direccion {recId}: {response.StatusCode} - {errorContent}");
                throw new Exception($"Error al actualizar direccion: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar direccion {recId}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("EmployeesAddress")}/{recId}";
                _logger.LogInformation($"Eliminando direccion: {recId}");

                var response = await _httpClient.DeleteAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Direccion {recId} eliminada exitosamente");
                    return true;
                }

                _logger.LogWarning($"Error al eliminar direccion {recId}: {response.StatusCode}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar direccion {recId}");
                return false;
            }
        }
    }
}
