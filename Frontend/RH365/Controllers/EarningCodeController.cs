// ============================================================================
// Archivo: EarningCodeController.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365/Controllers/EarningCodeController.cs
// Descripción: Controlador MVC para gestión de Códigos de Nómina
// Estándar: ISO 27001 - Control de acceso y trazabilidad
// ============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RH365.Infrastructure.Services;

namespace RH365.Controllers
{
    public class EarningCodeController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly ILogger<EarningCodeController> _logger;

        public EarningCodeController(IUserContext userContext, ILogger<EarningCodeController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        /// <summary>
        /// Vista de listado de códigos de nómina
        /// </summary>
        [HttpGet]
        public IActionResult LP_EarningCodes()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login");
            }

            ViewBag.DataareaID = _userContext.DataareaID;
            ViewBag.UserRefRecID = _userContext.UserRefRecID;
            ViewBag.CompanyName = _userContext.CompanyName ?? "RH-365";

            return View("~/Views/EarningCode/LP_EarningCodes.cshtml");
        }

        /// <summary>
        /// Vista de formulario para crear/editar código de nómina
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

            return View("~/Views/EarningCode/NewEdit_EarningCode.cshtml");
        }
    }
}
