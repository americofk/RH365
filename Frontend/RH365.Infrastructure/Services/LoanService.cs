// ============================================================================
// Archivo: LoanService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/LoanService.cs
// Descripción: Servicio para comunicación con API de Préstamos
// ============================================================================

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RH365.Core.Domain.Models.Loan;

namespace RH365.Infrastructure.Services
{
    public interface ILoanService
    {
        Task<LoanListResponse> GetAllAsync(int pageNumber = 1, int pageSize = 100, CancellationToken ct = default);
        Task<LoanResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<LoanResponse> CreateAsync(CreateLoanRequest request, CancellationToken ct = default);
        Task<LoanResponse> UpdateAsync(long recId, UpdateLoanRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
        Task<List<LoanResponse>> GetEnabledAsync(CancellationToken ct = default);
    }

    public class LoanService : ILoanService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<LoanService> _logger;

        public LoanService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<LoanService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        public async Task<LoanListResponse> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 100,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Loans")}?pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<LoanListResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new LoanListResponse();
                }

                _logger.LogWarning($"Error obteniendo préstamos: {response.StatusCode}");
                return new LoanListResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener préstamos");
                throw;
            }
        }

        public async Task<LoanResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Loans")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<LoanResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener préstamo {recId}");
                throw;
            }
        }

        public async Task<LoanResponse> CreateAsync(CreateLoanRequest request, CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var url = _urlsService.GetUrl("Loans");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<LoanResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al crear préstamo: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear préstamo");
                throw;
            }
        }

        public async Task<LoanResponse> UpdateAsync(long recId, UpdateLoanRequest request, CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var url = $"{_urlsService.GetUrl("Loans")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<LoanResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al actualizar préstamo: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar préstamo {recId}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("Loans")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar préstamo {recId}");
                return false;
            }
        }

        public async Task<List<LoanResponse>> GetEnabledAsync(CancellationToken ct = default)
        {
            try
            {
                var url = _urlsService.GetUrl("Loans.Enabled");
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<List<LoanResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<LoanResponse>();
                }

                return new List<LoanResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener préstamos activos");
                return new List<LoanResponse>();
            }
        }
    }
}
