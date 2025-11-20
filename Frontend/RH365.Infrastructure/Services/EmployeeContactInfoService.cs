// ============================================================================
// Archivo: EmployeeContactInfoService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/EmployeeContactInfoService.cs
// Descripcion:
//   - Servicio para comunicacion con API de Informacion de Contacto de Empleados
//   - Maneja CRUD completo
//   - Usa modelos de RH365.Core
// Estandar: ISO 27001 - Gestion segura de informacion de contacto
// ============================================================================

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RH365.Core.Domain.Models.EmployeeContactInfo;

namespace RH365.Infrastructure.Services
{
    public interface IEmployeeContactInfoService
    {
        Task<EmployeeContactInfoListResponse> GetAllAsync(int pageNumber = 1, int pageSize = 100, CancellationToken ct = default);
        Task<EmployeeContactInfoResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<List<EmployeeContactInfoResponse>> GetByEmployeeAsync(long employeeRefRecID, CancellationToken ct = default);
        Task<EmployeeContactInfoResponse> CreateAsync(CreateEmployeeContactInfoRequest request, CancellationToken ct = default);
        Task<EmployeeContactInfoResponse> UpdateAsync(long recId, UpdateEmployeeContactInfoRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
    }

    public class EmployeeContactInfoService : IEmployeeContactInfoService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<EmployeeContactInfoService> _logger;

        public EmployeeContactInfoService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<EmployeeContactInfoService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los contactos de empleados paginados
        /// </summary>
        public async Task<EmployeeContactInfoListResponse> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 100,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("EmployeesContactInfo")}?pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<EmployeeContactInfoListResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new EmployeeContactInfoListResponse();
                }

                _logger.LogWarning($"Error obteniendo contactos de empleados: {response.StatusCode}");
                return new EmployeeContactInfoListResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener contactos de empleados");
                throw;
            }
        }

        /// <summary>
        /// Obtener contacto de empleado por RecID
        /// </summary>
        public async Task<EmployeeContactInfoResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("EmployeesContactInfo")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<EmployeeContactInfoResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener contacto de empleado {recId}");
                throw;
            }
        }

        /// <summary>
        /// Obtener contactos por empleado especifico
        /// </summary>
        public async Task<List<EmployeeContactInfoResponse>> GetByEmployeeAsync(
            long employeeRefRecID,
            CancellationToken ct = default)
        {
            try
            {
                // Obtener todos los contactos y filtrar por empleado
                var allContacts = await GetAllAsync(1, 100, ct);
                var employeeContacts = allContacts.Data.FindAll(c => c.EmployeeRefRecID == employeeRefRecID);

                return employeeContacts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener contactos del empleado {employeeRefRecID}");
                return new List<EmployeeContactInfoResponse>();
            }
        }

        /// <summary>
        /// Crear nuevo contacto de empleado
        /// </summary>
        public async Task<EmployeeContactInfoResponse> CreateAsync(
            CreateEmployeeContactInfoRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("EmployeesContactInfo");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<EmployeeContactInfoResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al crear contacto de empleado: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear contacto de empleado");
                throw;
            }
        }

        /// <summary>
        /// Actualizar contacto de empleado existente
        /// </summary>
        public async Task<EmployeeContactInfoResponse> UpdateAsync(
            long recId,
            UpdateEmployeeContactInfoRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("EmployeesContactInfo")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<EmployeeContactInfoResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al actualizar contacto de empleado: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar contacto de empleado {recId}");
                throw;
            }
        }

        /// <summary>
        /// Eliminar contacto de empleado
        /// </summary>
        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("EmployeesContactInfo")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar contacto de empleado {recId}");
                return false;
            }
        }
    }
}
