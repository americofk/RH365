// ============================================================================
// Archivo: MenuService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/MenuService.cs
// Descripción:
//   - Servicio para comunicación con API de menús
//   - Construye jerarquía de menús
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        private readonly ILogger<MenuService> _logger;
        private readonly string _baseUrl;

        public MenuService(HttpClient httpClient, IConfiguration configuration, ILogger<MenuService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _baseUrl = _configuration["ApiSettings:BaseUrl"] ?? "http://localhost:9595/api";
        }

        /// <summary>
        /// Obtener menús del usuario
        /// </summary>
        public async Task<List<MenuItemResponse>> GetMenusAsync(string token)
        {
            try
            {
                // Configurar token
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                // Llamar API
                var response = await _httpClient.GetAsync($"{_baseUrl}/MenusApps");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var menus = JsonSerializer.Deserialize<List<MenuItemResponse>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    // Construir jerarquía
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
            // Solo menús visibles
            var visible = flatList.Where(m => m.IsViewMenu).ToList();

            // Menús raíz
            var roots = visible.Where(m => m.MenuFatherRefRecID == null)
                              .OrderBy(m => m.Sort)
                              .ToList();

            // Asignar hijos
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