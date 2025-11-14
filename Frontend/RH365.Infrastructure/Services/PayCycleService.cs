// ============================================================================
// Archivo: PayCycleService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/PayCycleService.cs
// Descripción:
//   - Servicio para comunicación con API de Ciclos de Pago
//   - Maneja CRUD completo y filtrado por nómina
//   - Usa modelos de RH365.Core
// ISO 27001: Control de acceso a datos de ciclos de pago con trazabilidad
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RH365.Core.Domain.Models.PayCycle;

namespace RH365.Infrastructure.Services
{
    public interface IPayCycleService
    {
        Task<List<PayCycleResponse>> GetAllAsync(CancellationToken ct = default);
        Task<List<PayCycleResponse>> GetByPayrollIdAsync(long payrollRefRecID, CancellationToken ct = default);
        Task<PayCycleResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<PayCycleResponse> CreateAsync(CreatePayCycleRequest request, CancellationToken ct = default);
        Task<PayCycleResponse> UpdateAsync(long recId, UpdatePayCycleRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
    }

    public class PayCycleService : IPayCycleService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<PayCycleService> _logger;

        public PayCycleService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<PayCycleService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los ciclos de pago
        /// </summary>
        public async Task<List<PayCycleResponse>> GetAllAsync(CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("PayCycles")}?skip=0&take=1000";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    
                    // Manejar tanto array directo como objeto con Data
                    var result = JsonSerializer.Deserialize<List<PayCycleResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new List<PayCycleResponse>();
                }

                _logger.LogWarning($"Error obteniendo ciclos de pago: {response.StatusCode}");
                return new List<PayCycleResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ciclos de pago");
                throw;
            }
        }

        /// <summary>
        /// Obtener ciclos de pago por nómina
        /// </summary>
        public async Task<List<PayCycleResponse>> GetByPayrollIdAsync(
            long payrollRefRecID, 
            CancellationToken ct = default)
        {
            try
            {
                var allCycles = await GetAllAsync(ct);
                
                // Filtrar por PayrollRefRecID
                return allCycles
                    .Where(c => c.PayrollRefRecID == payrollRefRecID)
                    .OrderByDescending(c => c.PeriodStartDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener ciclos de pago para nómina {payrollRefRecID}");
                throw;
            }
        }

        /// <summary>
        /// Obtener ciclo de pago por RecID
        /// </summary>
        public async Task<PayCycleResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("PayCycles")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<PayCycleResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener ciclo de pago {recId}");
                throw;
            }
        }

        /// <summary>
        /// Crear nuevo ciclo de pago
        /// </summary>
        public async Task<PayCycleResponse> CreateAsync(
            CreatePayCycleRequest request, 
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("PayCycles");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<PayCycleResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al crear ciclo de pago: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear ciclo de pago");
                throw;
            }
        }

        /// <summary>
        /// Actualizar ciclo de pago existente
        /// </summary>
        public async Task<PayCycleResponse> UpdateAsync(
            long recId,
            UpdatePayCycleRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("PayCycles")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<PayCycleResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al actualizar ciclo de pago: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar ciclo de pago {recId}");
                throw;
            }
        }

        /// <summary>
        /// Eliminar ciclo de pago
        /// </summary>
        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("PayCycles")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar ciclo de pago {recId}");
                return false;
            }
        }
    }
}
