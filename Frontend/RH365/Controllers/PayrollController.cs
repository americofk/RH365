// ============================================================================
// Archivo: PayrollController.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365.WebMVC/Controllers/PayrollController.cs
// Descripción: Controlador MVC para gestión de Nóminas
// ISO 27001: Control de acceso con validación de sesión y contexto de usuario
// ============================================================================
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RH365.Infrastructure.Services;

namespace RH365.Controllers
{
    public class PayrollController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly ILogger<PayrollController> _logger;

        public PayrollController(IUserContext userContext, ILogger<PayrollController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        /// <summary>
        /// Vista de listado de nóminas
        /// </summary>
        [HttpGet]
        public IActionResult LP_Payrolls()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login");
            }

            ViewBag.DataareaID = _userContext.DataareaID;
            ViewBag.UserRefRecID = _userContext.UserRefRecID;
            ViewBag.CompanyName = _userContext.CompanyName ?? "RH-365";

            return View("~/Views/Payroll/LP_Payrolls.cshtml");
        }

        /// <summary>
        /// Vista de crear/editar nómina
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

            return View("~/Views/Payroll/NewEdit_Payroll.cshtml");
        }
    }
}
