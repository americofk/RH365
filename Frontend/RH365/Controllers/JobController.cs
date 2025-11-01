// ============================================================================
// Archivo: JobController.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365/Controllers/JobController.cs
// Descripción:
//   - Controlador MVC para gestión de Cargos
//   - Maneja vistas de listado y formulario crear/editar
//   - Validación de autenticación y contexto de usuario
// Estándar: ISO 27001 - Control de acceso y autenticación
// ============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RH365.Infrastructure.Services;

namespace RH365.Controllers
{
    public class JobController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly ILogger<JobController> _logger;

        public JobController(IUserContext userContext, ILogger<JobController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        /// <summary>
        /// Vista de listado de cargos
        /// </summary>
        [HttpGet]
        public IActionResult LP_Jobs()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login");
            }

            ViewBag.DataareaID = _userContext.DataareaID;
            ViewBag.UserRefRecID = _userContext.UserRefRecID;
            ViewBag.CompanyName = _userContext.CompanyName ?? "RH-365";

            return View("~/Views/Job/LP_Jobs.cshtml");
        }

        /// <summary>
        /// Vista de formulario crear/editar cargo
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

            return View("~/Views/Job/NewEdit_Job.cshtml");
        }
    }
}
