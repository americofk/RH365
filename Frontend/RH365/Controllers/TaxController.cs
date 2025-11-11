// ============================================================================
// Archivo: TaxController.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365/Controllers/TaxController.cs
// Descripción:
//   - Controlador MVC para gestión de Impuestos
//   - Maneja vistas de listado y formulario (crear/editar)
//   - Integración con contexto de usuario
// Estándar: ISO 27001 - Control de acceso y autenticación
// ============================================================================
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RH365.Infrastructure.Services;

namespace RH365.Controllers
{
    public class TaxController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly ILogger<TaxController> _logger;

        public TaxController(IUserContext userContext, ILogger<TaxController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        /// <summary>
        /// Vista de listado de impuestos (ListPage)
        /// </summary>
        [HttpGet]
        public IActionResult LP_Taxes()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login");
            }

            ViewBag.DataareaID = _userContext.DataareaID;
            ViewBag.UserRefRecID = _userContext.UserRefRecID;
            ViewBag.CompanyName = _userContext.CompanyName ?? "RH-365";

            return View("~/Views/Tax/LP_Taxes.cshtml");
        }

        /// <summary>
        /// Vista de formulario para crear/editar impuestos
        /// </summary>
        [HttpGet]
        public IActionResult NewEdit(long? recId = null)
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login");
            }

            ViewBag.DataareaID = _userContext.DataareaID;
            ViewBag.UserRefRecID = _userContext.UserRefRecID;
            ViewBag.CompanyName = _userContext.CompanyName ?? "RH-365";
            ViewBag.RecID = recId ?? 0L;
            ViewBag.IsNew = !recId.HasValue || recId.Value <= 0;

            return View("~/Views/Tax/NewEdit_Tax.cshtml");
        }
    }
}
