// ============================================================================
// Archivo: EmployeeDocumentService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/EmployeeDocumentService.cs
// Descripcion:
//   - Servicio para comunicacion con API de Documentos de Empleados
//   - Maneja CRUD completo de documentos de identidad
//   - Soporta filtrado por empleado especifico
//   - Usa modelos de RH365.Core.Domain.Models.EmployeeDocument
// Estandar: ISO 27001 - Gestion segura de documentos de identidad
// ============================================================================

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RH365.Core.Domain.Models.EmployeeDocument;

namespace RH365.Infrastructure.Services
{
    public interface IEmployeeDocumentService
    {
        /// <summary>
        /// Obtener todos los documentos paginados
        /// </summary>
        Task<EmployeeDocumentListResponse> GetAllAsync(int pageNumber = 1, int pageSize = 100, CancellationToken ct = default);

        /// <summary>
        /// Obtener documento por RecID
        /// </summary>
        Task<EmployeeDocumentResponse> GetByIdAsync(long recId, CancellationToken ct = default);

        /// <summary>
        /// Obtener todos los documentos de un empleado especifico
        /// </summary>
        Task<List<EmployeeDocumentResponse>> GetByEmployeeAsync(long employeeRefRecID, CancellationToken ct = default);

        /// <summary>
        /// Crear nuevo documento
        /// </summary>
        Task<EmployeeDocumentResponse> CreateAsync(CreateEmployeeDocumentRequest request, CancellationToken ct = default);

        /// <summary>
        /// Actualizar documento existente
        /// </summary>
        Task<EmployeeDocumentResponse> UpdateAsync(long recId, UpdateEmployeeDocumentRequest request, CancellationToken ct = default);

        /// <summary>
        /// Eliminar documento
        /// </summary>
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
    }

    public class EmployeeDocumentService : IEmployeeDocumentService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<EmployeeDocumentService> _logger;

        public EmployeeDocumentService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<EmployeeDocumentService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los documentos paginados
        /// </summary>
        public async Task<EmployeeDocumentListResponse> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 100,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("EmployeeDocuments")}?pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<EmployeeDocumentListResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new EmployeeDocumentListResponse();
                }

                _logger.LogWarning($"Error obteniendo documentos: {response.StatusCode}");
                return new EmployeeDocumentListResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener documentos de empleados");
                throw;
            }
        }

        /// <summary>
        /// Obtener documento por RecID
        /// </summary>
        public async Task<EmployeeDocumentResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("EmployeeDocuments")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<EmployeeDocumentResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                _logger.LogWarning($"Documento no encontrado: {recId}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener documento {recId}");
                throw;
            }
        }

        /// <summary>
        /// Obtener todos los documentos de un empleado especifico
        /// </summary>
        public async Task<List<EmployeeDocumentResponse>> GetByEmployeeAsync(
            long employeeRefRecID,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("EmployeeDocuments")}/employee/{employeeRefRecID}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<List<EmployeeDocumentResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<EmployeeDocumentResponse>();
                }

                _logger.LogWarning($"No se encontraron documentos para empleado: {employeeRefRecID}");
                return new List<EmployeeDocumentResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener documentos del empleado {employeeRefRecID}");
                throw;
            }
        }

        /// <summary>
        /// Crear nuevo documento
        /// </summary>
        public async Task<EmployeeDocumentResponse> CreateAsync(
            CreateEmployeeDocumentRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("EmployeeDocuments");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<EmployeeDocumentResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                var errorContent = await response.Content.ReadAsStringAsync(ct);
                throw new Exception($"Error al crear documento: {response.StatusCode} - {errorContent}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear documento de empleado");
                throw;
            }
        }

        /// <summary>
        /// Actualizar documento existente
        /// </summary>
        public async Task<EmployeeDocumentResponse> UpdateAsync(
            long recId,
            UpdateEmployeeDocumentRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("EmployeeDocuments")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<EmployeeDocumentResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                var errorContent = await response.Content.ReadAsStringAsync(ct);
                throw new Exception($"Error al actualizar documento: {response.StatusCode} - {errorContent}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar documento {recId}");
                throw;
            }
        }

        /// <summary>
        /// Eliminar documento
        /// </summary>
        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("EmployeeDocuments")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Documento eliminado exitosamente: {recId}");
                    return true;
                }

                _logger.LogWarning($"Error al eliminar documento {recId}: {response.StatusCode}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar documento {recId}");
                return false;
            }
        }
    }
}
