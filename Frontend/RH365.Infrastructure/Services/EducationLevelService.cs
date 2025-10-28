// ============================================================================
// Archivo: EducationLevelService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/EducationLevelService.cs
// Descripción:
//   Cliente HTTP para el recurso "Niveles Educativos".
//   Endpoints esperados:
//     GET    /api/EducationLevels
//     GET    /api/EducationLevels/{recId}
//     POST   /api/EducationLevels
//     PUT    /api/EducationLevels/{recId}
//     DELETE /api/EducationLevels/{recId}
//
// Dependencias:
//   - IHttpClientFactory (registrado en DI).
//   - IUrlsService (resuelve las rutas).
//   - Modelos en RH365.Core.Domain.Models.EducationLevel.*
// ============================================================================
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using RH365.Core.Domain.Models.EducationLevel;

namespace RH365.Infrastructure.Services
{
    public sealed class EducationLevelService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUrlsService _urls;

        private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public EducationLevelService(IHttpClientFactory httpClientFactory, IUrlsService urls)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _urls = urls ?? throw new ArgumentNullException(nameof(urls));
        }

        // ------------------- Query -------------------

        public async Task<EducationLevelListResponse> GetAllAsync(int pageNumber, int pageSize, string? bearerToken)
        {
            var client = CreateClient(bearerToken);
            var url = $"{_urls.GetUrl("EducationLevels")}?pageNumber={pageNumber}&pageSize={pageSize}";

            using var resp = await client.GetAsync(url).ConfigureAwait(false);
            if (!resp.IsSuccessStatusCode) throw await BuildHttpExceptionAsync("GET", url, resp).ConfigureAwait(false);

            var payload = await resp.Content.ReadFromJsonAsync<EducationLevelListResponse>(JsonOpts).ConfigureAwait(false);
            return payload ?? new EducationLevelListResponse();
        }

        public async Task<EducationLevelResponse> GetByIdAsync(long recId, string? bearerToken)
        {
            var client = CreateClient(bearerToken);
            var url = $"{_urls.GetUrl("EducationLevels")}/{recId}";

            using var resp = await client.GetAsync(url).ConfigureAwait(false);
            if (!resp.IsSuccessStatusCode) throw await BuildHttpExceptionAsync("GET", url, resp).ConfigureAwait(false);

            var payload = await resp.Content.ReadFromJsonAsync<EducationLevelResponse>(JsonOpts).ConfigureAwait(false);
            return payload ?? new EducationLevelResponse();
        }

        // ------------------- Commands -------------------

        public async Task<EducationLevelResponse> CreateAsync(CreateEducationLevelRequest request, string? bearerToken)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var client = CreateClient(bearerToken);
            var url = _urls.GetUrl("EducationLevels");

            using var resp = await client.PostAsJsonAsync(url, request, JsonOpts).ConfigureAwait(false);
            if (!resp.IsSuccessStatusCode) throw await BuildHttpExceptionAsync("POST", url, resp).ConfigureAwait(false);

            var payload = await resp.Content.ReadFromJsonAsync<EducationLevelResponse>(JsonOpts).ConfigureAwait(false);
            return payload ?? new EducationLevelResponse();
        }

        public async Task<EducationLevelResponse> UpdateAsync(long recId, UpdateEducationLevelRequest request, string? bearerToken)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var client = CreateClient(bearerToken);
            var url = $"{_urls.GetUrl("EducationLevels")}/{recId}";

            using var resp = await client.PutAsJsonAsync(url, request, JsonOpts).ConfigureAwait(false);
            if (!resp.IsSuccessStatusCode) throw await BuildHttpExceptionAsync("PUT", url, resp).ConfigureAwait(false);

            var payload = await resp.Content.ReadFromJsonAsync<EducationLevelResponse>(JsonOpts).ConfigureAwait(false);
            return payload ?? new EducationLevelResponse();
        }

        public async Task<bool> DeleteAsync(long recId, string? bearerToken)
        {
            var client = CreateClient(bearerToken);
            var url = $"{_urls.GetUrl("EducationLevels")}/{recId}";

            using var resp = await client.DeleteAsync(url).ConfigureAwait(false);
            if (!resp.IsSuccessStatusCode) throw await BuildHttpExceptionAsync("DELETE", url, resp).ConfigureAwait(false);

            return true;
        }

        // ------------------- Helpers -------------------

        private HttpClient CreateClient(string? bearerToken)
        {
            var client = _httpClientFactory.CreateClient(nameof(EducationLevelService));
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrWhiteSpace(bearerToken))
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

            return client;
        }

        private static async Task<InvalidOperationException> BuildHttpExceptionAsync(string verb, string url, HttpResponseMessage resp)
        {
            string detail;
            try
            {
                detail = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (string.IsNullOrWhiteSpace(detail)) detail = resp.ReasonPhrase ?? "Sin detalle";
            }
            catch
            {
                detail = resp.ReasonPhrase ?? "Sin detalle";
            }
            var msg = $"Error {verb} {url} → {(int)resp.StatusCode} {resp.StatusCode}. Detalle: {detail}";
            return new InvalidOperationException(msg);
        }
    }
}
