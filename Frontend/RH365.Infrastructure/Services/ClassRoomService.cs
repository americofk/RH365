// ============================================================================
// Archivo: ClassRoomService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/ClassRoomService.cs
// Descripción:
//   - Servicio para comunicación con API de Salones de Clases
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
using RH365.Core.Domain.Models.ClassRoom;

namespace RH365.Infrastructure.Services
{
    public interface IClassRoomService
    {
        Task<List<ClassRoomResponse>> GetAllAsync(int skip = 0, int take = 100, CancellationToken ct = default);
        Task<ClassRoomResponse> GetByIdAsync(long recId, CancellationToken ct = default);
        Task<ClassRoomResponse> CreateAsync(CreateClassRoomRequest request, CancellationToken ct = default);
        Task<ClassRoomResponse> UpdateAsync(long recId, UpdateClassRoomRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(long recId, CancellationToken ct = default);
    }

    public class ClassRoomService : IClassRoomService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<ClassRoomService> _logger;

        public ClassRoomService(
            HttpClient httpClient,
            IUrlsService urlsService,
            ILogger<ClassRoomService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los salones de clases
        /// </summary>
        public async Task<List<ClassRoomResponse>> GetAllAsync(
            int skip = 0,
            int take = 100,
            CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("ClassRooms")}?skip={skip}&take={take}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    var result = JsonSerializer.Deserialize<List<ClassRoomResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new List<ClassRoomResponse>();
                }

                _logger.LogWarning($"Error obteniendo salones de clases: {response.StatusCode}");
                return new List<ClassRoomResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener salones de clases");
                throw;
            }
        }

        /// <summary>
        /// Obtener salón de clases por RecID
        /// </summary>
        public async Task<ClassRoomResponse> GetByIdAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("ClassRooms")}/{recId}";
                var response = await _httpClient.GetAsync(url, ct);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<ClassRoomResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener salón de clases {recId}");
                throw;
            }
        }

        /// <summary>
        /// Crear nuevo salón de clases
        /// </summary>
        public async Task<ClassRoomResponse> CreateAsync(CreateClassRoomRequest request, CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _urlsService.GetUrl("ClassRooms");
                var response = await _httpClient.PostAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<ClassRoomResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al crear salón de clases: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear salón de clases");
                throw;
            }
        }

        /// <summary>
        /// Actualizar salón de clases existente
        /// </summary>
        public async Task<ClassRoomResponse> UpdateAsync(
            long recId,
            UpdateClassRoomRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_urlsService.GetUrl("ClassRooms")}/{recId}";
                var response = await _httpClient.PutAsync(url, content, ct);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync(ct);
                    return JsonSerializer.Deserialize<ClassRoomResponse>(responseJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                throw new Exception($"Error al actualizar salón de clases: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar salón de clases {recId}");
                throw;
            }
        }

        /// <summary>
        /// Eliminar salón de clases
        /// </summary>
        public async Task<bool> DeleteAsync(long recId, CancellationToken ct = default)
        {
            try
            {
                var url = $"{_urlsService.GetUrl("ClassRooms")}/{recId}";
                var response = await _httpClient.DeleteAsync(url, ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar salón de clases {recId}");
                return false;
            }
        }
    }
}
