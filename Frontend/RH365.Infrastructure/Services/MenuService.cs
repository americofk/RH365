// ============================================================================
// Archivo: MenuService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/MenuService.cs
// Descripción:
//   - Servicio para comunicación con API de menús
//   - Usa UrlsService centralizado
//   - Construye jerarquía de menús
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RH365.Core.Domain.Models.Menu;

namespace RH365.Infrastructure.Services
{
    /// <summary>
    /// Servicio para gestión de menús
    /// </summary>
    public class MenuService
    {
        private readonly HttpClient _httpClient;
        private readonly IUrlsService _urlsService;
        private readonly ILogger<MenuService> _logger;

        public MenuService(HttpClient httpClient, IUrlsService urlsService, ILogger<MenuService> logger)
        {
            _httpClient = httpClient;
            _urlsService = urlsService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener menús del usuario
        /// </summary>
        public async Task<List<MenuItemResponse>> GetMenusAsync(string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var url = _urlsService.GetUrl("MenusApps");
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var menus = JsonSerializer.Deserialize<List<MenuItemResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return BuildHierarchy(menus ?? new List<MenuItemResponse>());
                }

                _logger.LogWarning($"Error obteniendo menús: {response.StatusCode}");
                return new List<MenuItemResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener menús");
                return new List<MenuItemResponse>();
            }
        }

        /// <summary>
        /// Construir jerarquía de menús
        /// </summary>
        private List<MenuItemResponse> BuildHierarchy(List<MenuItemResponse> flatList)
        {
            var visible = flatList.Where(m => m.IsViewMenu).ToList();

            var roots = visible.Where(m => m.MenuFatherRefRecID == null)
                              .OrderBy(m => m.Sort)
                              .ToList();

            foreach (var root in roots)
            {
                root.Children = GetChildren(root.RecID, visible);
            }

            return roots;
        }

        /// <summary>
        /// Obtener hijos recursivamente
        /// </summary>
        private List<MenuItemResponse> GetChildren(long parentId, List<MenuItemResponse> all)
        {
            var children = all.Where(m => m.MenuFatherRefRecID == parentId)
                             .OrderBy(m => m.Sort)
                             .ToList();

            foreach (var child in children)
            {
                child.Children = GetChildren(child.RecID, all);
            }

            return children;
        }
    }
}