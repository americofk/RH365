// ============================================================================
// Archivo: PayrollService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/PayrollService.cs
// Descripción:
//   - Servicio para comunicación con API de Nóminas
//   - Maneja CRUD completo y obtención de nóminas activas
//   - Usa modelos de RH365.Core
// ISO 27001: Control de acceso a datos de nómina con trazabilidad
// ============================================================================

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RH365.Core.Domain.Models.Payroll;

namespace RH365.Infrastructure.Services
{
    public interface IPayrollService
    {
        Task<PayrollListResponse> GetAllAsync(int pageNumber = 1, int pageSize = 100, CancellationToken ct = default);
        Task<PayrollResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<PayrollResponse> CreateAsync(CreatePayrollRequest request, CancellationToken ct = default);
        Task<PayrollResponse> UpdateAsync(long recId, UpdatePayrollRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
        Task<List<PayrollResponse>> GetEnabledAsync(CancellationToken ct = default);
    }

    public class PayrollService : IPayrollService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<PayrollService> _logger;

        public PayrollService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<PayrollService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todas las nóminas paginadas
        /// </summary>
        public async Task<PayrollListResponse> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 100,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Payrolls")}?pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<PayrollListResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new PayrollListResponse();
                }

                _logger.LogWarning($"Error obteniendo nóminas: {response.StatusCode}");
                return new PayrollListResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener nóminas");
                throw;
            }
        }

        /// <summary>
        /// Obtener nómina por RecID
        /// </summary>
        public async Task<PayrollResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Payrolls")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<PayrollResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener nómina {recId}");
                throw;
            }
        }

        /// <summary>
        /// Crear nueva nómina
        /// </summary>
        public async Task<PayrollResponse> CreateAsync(CreatePayrollRequest request, CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("Payrolls");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<PayrollResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al crear nómina: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nómina");
                throw;
            }
        }

        /// <summary>
        /// Actualizar nómina existente
        /// </summary>
        public async Task<PayrollResponse> UpdateAsync(
            long recId,
            UpdatePayrollRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("Payrolls")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<PayrollResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al actualizar nómina: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar nómina {recId}");
                throw;
            }
        }

        /// <summary>
        /// Eliminar nómina
        /// </summary>
        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Payrolls")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar nómina {recId}");
                return false;
            }
        }

        /// <summary>
        /// Obtener nóminas activas
        /// </summary>
        public async Task<List<PayrollResponse>> GetEnabledAsync(CancellationToken ct = default)
        {
            try
            {
                var url = _urlsService.GetUrl("Payrolls.Enabled");
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<List<PayrollResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<PayrollResponse>();
                }

                return new List<PayrollResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener nóminas activas");
                return new List<PayrollResponse>();
            }
        }
    }
}
