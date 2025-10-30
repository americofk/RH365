// ============================================================================
// Archivo: CalendarHolidayService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/CalendarHolidayService.cs
// Descripción:
//   - Servicio para comunicación con API de Días Feriados
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
using RH365.Core.Domain.Models.CalendarHoliday;

namespace RH365.Infrastructure.Services
{
    public interface ICalendarHolidayService
    {
        Task<CalendarHolidayListResponse> GetAllAsync(int pageNumber = 1, int pageSize = 100, CancellationToken ct = default);
        Task<CalendarHolidayResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<CalendarHolidayResponse> CreateAsync(CreateCalendarHolidayRequest request, CancellationToken ct = default);
        Task<CalendarHolidayResponse> UpdateAsync(long recId, UpdateCalendarHolidayRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
    }

    public class CalendarHolidayService : ICalendarHolidayService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<CalendarHolidayService> _logger;

        public CalendarHolidayService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<CalendarHolidayService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los días feriados paginados
        /// </summary>
        public async Task<CalendarHolidayListResponse> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 100,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("CalendarHolidays")}?pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<CalendarHolidayListResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new CalendarHolidayListResponse();
                }

                _logger.LogWarning($"Error obteniendo días feriados: {response.StatusCode}");
                return new CalendarHolidayListResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener días feriados");
                throw;
            }
        }

        /// <summary>
        /// Obtener día feriado por RecID
        /// </summary>
        public async Task<CalendarHolidayResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("CalendarHolidays")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<CalendarHolidayResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener día feriado {recId}");
                throw;
            }
        }

        /// <summary>
        /// Crear nuevo día feriado
        /// </summary>
        public async Task<CalendarHolidayResponse> CreateAsync(CreateCalendarHolidayRequest request, CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("CalendarHolidays");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<CalendarHolidayResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al crear día feriado: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear día feriado");
                throw;
            }
        }

        /// <summary>
        /// Actualizar día feriado existente
        /// </summary>
        public async Task<CalendarHolidayResponse> UpdateAsync(
            long recId,
            UpdateCalendarHolidayRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("CalendarHolidays")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<CalendarHolidayResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al actualizar día feriado: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar día feriado {recId}");
                throw;
            }
        }

        /// <summary>
        /// Eliminar día feriado
        /// </summary>
        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("CalendarHolidays")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar día feriado {recId}");
                return false;
            }
        }
    }
}
