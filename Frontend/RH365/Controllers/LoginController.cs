// ============================================================================
// Archivo: LoginController.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365/Controllers/LoginController.cs
// Descripción:
//   - Controlador MVC para autenticación
//   - Maneja login, logout y gestión de sesión
//   - Carga menús del usuario al hacer login
// ============================================================================

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RH365.Core.Domain.Models.Auth;
using RH365.Infrastructure.Services;
using System.Collections.Generic;
using System.Linq;

namespace RH365.Controllers
{
    /// <summary>
    /// Controlador para gestión de autenticación
    /// </summary>
    public class LoginController : Controller
    {
        private readonly AuthService _authService;
        private readonly MenuService _menuService;
        private readonly ILogger<LoginController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginController(
            AuthService authService,
            MenuService menuService,
            ILogger<LoginController> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _menuService = menuService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Mostrar vista de login
        /// </summary>
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            // Si ya está autenticado, redirigir a Home
            if (HttpContext.Session.GetString("Token") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// Procesar login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest model, string? returnUrl = null)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // Llamar al servicio de autenticación
                var response = await _authService.LoginAsync(model);

                if (response != null && !string.IsNullOrEmpty(response.Token))
                {
                    // Guardar en sesión - datos del usuario
                    HttpContext.Session.SetString("Token", response.Token);
                    HttpContext.Session.SetString("UserId", response.User.ID.ToString());
                    HttpContext.Session.SetString("UserName", response.User.Name);
                    HttpContext.Session.SetString("UserEmail", response.User.Email);
                    HttpContext.Session.SetString("UserAlias", response.User.Alias);

                    // Verificar y guardar empresa por defecto
                    if (response.User.DefaultCompany != null)
                    {
                        HttpContext.Session.SetString("CompanyId", response.User.DefaultCompany.Code);
                        HttpContext.Session.SetString("CompanyName", response.User.DefaultCompany.Name);
                        HttpContext.Session.SetString("CompanyRecId", response.User.DefaultCompany.Id);
                    }
                    else
                    {
                        _logger.LogError($"Usuario {response.User.Email} no tiene empresa asignada");
                        ModelState.AddModelError(string.Empty, "Usuario no tiene empresa asignada. Contacte al administrador.");
                        return View(model);
                    }

                    // CARGAR MENÚS DEL USUARIO
                    try
                    {
                        var menus = await _menuService.GetMenusAsync(response.Token);
                        if (menus != null && menus.Count > 0)
                        {
                            var menusJson = System.Text.Json.JsonSerializer.Serialize(menus);
                            HttpContext.Session.SetString("UserMenus", menusJson);
                            _logger.LogInformation($"Se cargaron {menus.Count} menús para el usuario {response.User.Email}");
                        }
                        else
                        {
                            _logger.LogWarning($"No se encontraron menús para el usuario {response.User.Email}");
                        }
                    }
                    catch (Exception menuEx)
                    {
                        _logger.LogError(menuEx, "Error al cargar menús del usuario");
                        // Continuar sin menús, no es crítico
                    }

                    // Guardar lista de empresas autorizadas si existen
                    if (response.User.AuthorizedCompanies != null && response.User.AuthorizedCompanies.Count > 0)
                    {
                        var companiesJson = System.Text.Json.JsonSerializer.Serialize(response.User.AuthorizedCompanies);
                        HttpContext.Session.SetString("AuthorizedCompanies", companiesJson);
                    }

                    // Calcular fecha de expiración
                    var expiresAt = DateTime.UtcNow.AddSeconds(response.ExpiresIn);
                    HttpContext.Session.SetString("TokenExpires", expiresAt.ToString("o"));

                    // Guardar token en cookie httpOnly para mayor seguridad
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = Request.IsHttps,
                        SameSite = SameSiteMode.Strict,
                        Expires = expiresAt
                    };
                    Response.Cookies.Append("AuthToken", response.Token, cookieOptions);

                    _logger.LogInformation($"Usuario {response.User.Email} autenticado exitosamente en empresa {response.User.DefaultCompany.Name}");

                    // Verificar si requiere cambio de contraseña
                    if (response.User.RequiresPasswordChange)
                    {
                        return RedirectToAction("ChangePassword", "Account");
                    }

                    // Redirigir
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Credenciales inválidas");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el login");
                ModelState.AddModelError(string.Empty, "Error al iniciar sesión. Por favor intente nuevamente.");
                return View(model);
            }
        }

        /// <summary>
        /// Cerrar sesión
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userEmail = HttpContext.Session.GetString("UserEmail");

                // Limpiar servicio
                await _authService.LogoutAsync();

                // Limpiar sesión
                HttpContext.Session.Clear();

                // Limpiar cookies
                Response.Cookies.Delete("AuthToken");

                _logger.LogInformation($"Usuario {userEmail} cerró sesión");

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante logout");
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Cerrar sesión por GET (solo desarrollo)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> LogoutGet()
        {
            await _authService.LogoutAsync();
            HttpContext.Session.Clear();
            Response.Cookies.Delete("AuthToken");
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Cambiar empresa (multiempresa)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SwitchCompany(string companyCode)  // AGREGAR async Task
        {
            try
            {
                var token = HttpContext.Session.GetString("Token");
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login");
                }

                // Obtener lista de empresas autorizadas
                var companiesJson = HttpContext.Session.GetString("AuthorizedCompanies");
                if (string.IsNullOrEmpty(companiesJson))
                {
                    _logger.LogWarning("No hay empresas autorizadas en sesión");
                    return RedirectToAction("Index", "Home");
                }

                var companies = System.Text.Json.JsonSerializer.Deserialize<List<CompanyInfo>>(companiesJson);
                var selectedCompany = companies?.FirstOrDefault(c => c.Code == companyCode);

                if (selectedCompany == null)
                {
                    _logger.LogWarning($"Empresa {companyCode} no autorizada para el usuario");
                    return RedirectToAction("Index", "Home");
                }

                // Actualizar empresa en sesión
                HttpContext.Session.SetString("CompanyId", selectedCompany.Code);
                HttpContext.Session.SetString("CompanyName", selectedCompany.Name);
                HttpContext.Session.SetString("CompanyRecId", selectedCompany.Id);

                // RECARGAR MENÚS PARA LA NUEVA EMPRESA
                try
                {
                    var menus = await _menuService.GetMenusAsync(token);  // AHORA SÍ PUEDE USAR await
                    if (menus != null && menus.Count > 0)
                    {
                        var menusJson = System.Text.Json.JsonSerializer.Serialize(menus);
                        HttpContext.Session.SetString("UserMenus", menusJson);
                    }
                }
                catch (Exception menuEx)
                {
                    _logger.LogError(menuEx, "Error al recargar menús tras cambio de empresa");
                }

                _logger.LogInformation($"Usuario cambió a empresa {selectedCompany.Name}");

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar empresa");
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Obtener lista de empresas disponibles
        /// </summary>
        [HttpGet]
        public IActionResult GetAvailableCompanies()
        {
            var companiesJson = HttpContext.Session.GetString("AuthorizedCompanies");
            if (string.IsNullOrEmpty(companiesJson))
            {
                return Json(new List<CompanyInfo>());
            }

            var companies = System.Text.Json.JsonSerializer.Deserialize<List<CompanyInfo>>(companiesJson);
            return Json(companies);
        }
    }
}